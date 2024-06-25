using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Models;
using ShoppingCartService.Services;
using ShoppingCartService.Exceptions;
using Xunit;

namespace ShoppingCartService.Tests
{
    public class CartServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetCartAsync_ShouldCreateNewCartIfNotExists()
        {
            // Arrange
            using var context = CreateContext();
            var service = new CartService(context);

            // Act
            var result = await service.GetCartAsync("user1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.UserId);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldAddItemToCart()
        {
            // Arrange
            using var context = CreateContext();
            var service = new CartService(context);
            var item = new Item { Name = "Test Item", Price = 10.99m, StockQuantity = 5 };
            context.Items.Add(item);
            await context.SaveChangesAsync();

            // Act
            var result = await service.AddItemToCartAsync("user1", item.Id, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.ItemId);
            Assert.Equal(2, result.Quantity);

            var updatedItem = await context.Items.FindAsync(item.Id);
            Assert.Equal(3, updatedItem.StockQuantity);
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldThrowExceptionWhenOutOfStock()
        {
            // Arrange
            using var context = CreateContext();
            var service = new CartService(context);
            var item = new Item { Name = "Test Item", Price = 10.99m, StockQuantity = 5 };
            context.Items.Add(item);
            await context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<OutOfStockException>(() =>
                service.AddItemToCartAsync("user1", item.Id, 10));
        }
    }
}