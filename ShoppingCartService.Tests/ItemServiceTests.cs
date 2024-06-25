using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Models;
using ShoppingCartService.Services;
using Xunit;

namespace ShoppingCartService.Tests
{
    public class ItemServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddItemAsync_ShouldAddNewItem()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ItemService(context);
            var item = new Item { Name = "Test Item", Price = 10.99m, StockQuantity = 5 };

            // Act
            var result = await service.AddItemAsync(item);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Item", result.Name);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ItemService(context);
            await context.Items.AddRangeAsync(
                new Item { Name = "Item 1", Price = 10.99m, StockQuantity = 5 },
                new Item { Name = "Item 2", Price = 20.99m, StockQuantity = 3 }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllItemsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetItemByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ItemService(context);
            var item = new Item { Name = "Test Item", Price = 10.99m, StockQuantity = 5 };
            context.Items.Add(item);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetItemByIdAsync(item.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Item", result.Name);
        }
    }
}