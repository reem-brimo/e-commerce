
using E_Commerce.Data.Models;
using E_Commerce.Data.Models.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<UserSet, RoleSet, int,
        UserClaimSet, UserRoleSet, UserloginSet, RoleClaimSet, UserTokenSet>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var mutableForeignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                mutableForeignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            builder.Entity<UserRoleSet>(userRole =>
            {   
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired();

                userRole.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired();

            });

            builder.Entity<RoleClaimSet>().ToTable("RoleClaims");
            builder.Entity<RoleSet>().ToTable("Roles");
            builder.Entity<UserClaimSet>().ToTable("UserClaims");
            builder.Entity<UserloginSet>().ToTable("Userlogins");
            builder.Entity<UserRoleSet>().ToTable("UserRoles");
            builder.Entity<UserSet>().ToTable("Users");
            builder.Entity<UserTokenSet>().ToTable("UserTokens");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

}
