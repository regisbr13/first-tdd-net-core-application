using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_NewItem_ShouldUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test product", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            order.TotalValue.Should().Be(200);
        }

        [Fact]
        public void AddItem_ExistentItem_ShouldUpdateItemQuantity()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Test product", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test product", 1, 100);
            order.AddItem(orderItem1);

            // Act
            order.AddItem(orderItem2);

            // Assert
            order.OrderItems.FirstOrDefault(i => i.ProductId == orderItem1.ProductId).Quantity.Should().Be(3);
            order.OrderItems.Count(i => i.ProductId == orderItem1.ProductId).Should().Be(1);
            order.TotalValue.Should().Be(300);
        }
    }
}
