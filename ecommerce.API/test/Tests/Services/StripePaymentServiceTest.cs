using E_Commerce.Data.Configuration;
using E_Commerce.Infrastructure.Factories;
using E_Commerce.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Stripe;

namespace Tests.Services
{
    public class StripePaymentServiceTest
    {
        [Fact]
        public void GetPaymentService_ShouldReturnStripeService_WhenPaymentMethodIsStripe()
        {
            // Arrange
            var stripeSecretKey = "sk_test_51Qw0iCA7yv3ka39HZix3igNQFNg0gD22cFwyytmlG7WxRxBGrvqzJhGTFa8kX946ZI1QDm1tpTDbxPOc7qq6g4OS00AhUyBAni";
            var stripeSettings = new StripeSettings { SecretKey = stripeSecretKey };

            // Mock IOptions<StripeSettings>
            var mockStripeOptions = new Mock<IOptions<StripeSettings>>();
            mockStripeOptions.Setup(x => x.Value).Returns(stripeSettings);

            // Mock IServiceProvider to return StripePaymentService
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(StripePaymentService)))
                .Returns(new StripePaymentService(stripeSecretKey));

            // Create the factory with the mocked IServiceProvider
            var factory = new PaymentServiceFactory(mockServiceProvider.Object);

            // Act
            var paymentService = factory.GetPaymentService("Stripe");

            // Assert
            Assert.IsType<StripePaymentService>(paymentService);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnSuccess_WhenPaymentIsProcessed()
        {
            // Arrange
            var stripeSecretKey = "sk_test_51Qw0iCA7yv3ka39HZix3igNQFNg0gD22cFwyytmlG7WxRxBGrvqzJhGTFa8kX946ZI1QDm1tpTDbxPOc7qq6g4OS00AhUyBAni";
            var stripePaymentService = new StripePaymentService(stripeSecretKey);

            var mockChargeService = new Mock<ChargeService>();
            var mockCharge = new Charge
            {
                Id = "ch_123456789", // Mock charge ID
                Paid = true, // Payment succeeded
                Status = "succeeded"
            };

            // Mock the ChargeService to return a successful charge
            mockChargeService
                .Setup(x => x.CreateAsync(It.IsAny<ChargeCreateOptions>(), null, default))
                .ReturnsAsync(mockCharge);

            // Inject the mock ChargeService into the StripePaymentService
            // Note: This requires modifying the StripePaymentService to accept a ChargeService dependency
            stripePaymentService.SetChargeService(mockChargeService.Object);

            // Act
            var result = await stripePaymentService.ProcessPaymentAsync(
                amount: 1000, // $10.00
                currency: "usd",
                description: "Test Payment",
                customerEmail: "test@example.com",
                token: "tok_visa" // Test token
            );

            // Assert
            Assert.True(result.Success);
            //Assert.Equal("ch_123456789", result.TransactionId);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnFailure_WhenPaymentFails()
        {
            // Arrange
            var stripeSecretKey = "sk_test_51Qw0iCA7yv3ka39HZix3igNQFNg0gD22cFwyytmlG7WxRxBGrvqzJhGTFa8kX946ZI1QDm1tpTDbxPOc7qq6g4OS00AhUyBAni";
            var stripePaymentService = new StripePaymentService(stripeSecretKey);

            var mockChargeService = new Mock<ChargeService>();

            // Mock the ChargeService to throw an exception
            mockChargeService
                .Setup(x => x.CreateAsync(It.IsAny<ChargeCreateOptions>(), null, default))
                .ThrowsAsync(new StripeException("Card declined"));

            // Inject the mock ChargeService into the StripePaymentService
            stripePaymentService.SetChargeService(mockChargeService.Object);

            // Act
            var result = await stripePaymentService.ProcessPaymentAsync(
                amount: 1000, // $10.00
                currency: "usd",
                description: "Test Payment",
                customerEmail: "test@example.com",
                token: "tok_chargeDeclined" // Test token for declined card
            );

            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Your card was declined.", result.ErrorMessage);
        }
    }
}
