using E_Commerce.Data.Models;
using E_Commerce.Data.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Seed
{
    public static class SeedData
    {
        public async static Task EnsureDatabaseExistAsync(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }


        public static async Task Initialize(IServiceCollection services)
        {

            var provider = services.BuildServiceProvider();
            var context = provider.GetService<ApplicationDbContext>();

            // Ensure the database is created
            await EnsureDatabaseExistAsync(context);

            if (!context.Set<UserSet>().Any())
            {
                await SeedUsers(services, context);
            }

            if (!context.Set<Product>().Any())
            {
                var products = new List<Product>();
                for (int i = 1; i <= 10; i++)
                {
                    products.Add(new Product
                    {
                        Name = $"Product {i}",
                        Price = 10.0m * i,
                        Description = $"Description for Product {i}",
                        Stock = 100 + i,
                        ImageUrl = $"https://example.com/product{i}.jpg"
                    });
                }
                context.AddRange(products);
                await context.SaveChangesAsync();
            }

            if (!context.Set<Order>().Any())
            {
                var rand = new Random();

                var users = context.Users.ToList();
                var orders = new List<Order>();
                for (int i = 1; i <= 10; i++)
                {
                    int index = rand.Next(users.Count);

                    orders.Add(new Order
                    {
                        UserId = users[index].Id,
                        OrderDate = DateTime.Now.AddDays(-i),
                        TotalAmount = 0 // Calculated later based on OrderItems
                    });
                }
                context.AddRange(orders);
                await context.SaveChangesAsync();
            }

            if (!context.Set<OrderItem>().Any())
            {
                var orders = context.Orders.ToList();
                var products = context.Products.ToList();
                var random = new Random();
                var orderItems = new List<OrderItem>();
                foreach (var order in orders)
                {
                    for (int i = 0; i < 3; i++) // Each order will have 3 items
                    {
                        var product = products[random.Next(products.Count)];
                        var quantity = random.Next(1, 5);
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = quantity,
                            Price = (float)(product.Price * quantity)
                        };
                        orderItems.Add(orderItem);
                        order.TotalAmount += (int)orderItem.Price;
                    }
                }
                context.AddRange(orderItems);
                context.UpdateRange(orders);

                await context.SaveChangesAsync();
            }

        }

        private static async Task SeedUsers(IServiceCollection services, ApplicationDbContext context)
        {
            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserSet>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleSet>>();

            // Seed Roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new RoleSet { Name = role });
                }
            }

            // Seed Admin User
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new UserSet
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    if (await roleManager.RoleExistsAsync("Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }

            // Seed Regular Users
            for (int i = 1; i <= 9; i++)
            {
                var userEmail = $"user{i}@example.com";
                var userPassword = "User@123";

                if (await userManager.FindByEmailAsync(userEmail) == null)
                {
                    var user = new UserSet
                    {
                        UserName = userEmail,
                        Email = userEmail
                    };

                    var result = await userManager.CreateAsync(user, userPassword);
                    if (result.Succeeded)
                    {
                        if (await roleManager.RoleExistsAsync("User"))
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }
                    }
                }
            }
            await context.SaveChangesAsync();

        }
    }
}