using FluentAssertions;
using NerdStore.Core.Exceptions.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class OrderTests
    {
        // Add Item:
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

        [Fact]
        public void AddItem_ItemUnitsAboveAllowed_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test product", Order.MAX_PRODUCT_QUANTITY + 1, 100);

            // Act
            Action result = () => order.AddItem(orderItem);

            // Assert
            result.Should().Throw<DomainException>().WithMessage($"Maximum of {Order.MAX_PRODUCT_QUANTITY} units per product.");
        }

        [Fact]
        public void AddItem_ItemExistentUnitsAboveAllowed_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Test product", Order.MAX_PRODUCT_QUANTITY, 100);
            var orderItem2 = new OrderItem(productId, "Test product", 1, 100);
            order.AddItem(orderItem1);

            // Act
            Action result = () => order.AddItem(orderItem2);

            // Assert
            result.Should().Throw<DomainException>().WithMessage($"Maximum of {Order.MAX_PRODUCT_QUANTITY} units per product.");
        }

        [Fact]
        public void AddItem_ItemUnitsBellowAllowed_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test product", Order.MIN_PRODUCT_QUANTITY - 1, 100);

            // Act
            Action result = () => order.AddItem(orderItem);

            // Assert
            result.Should().Throw<DomainException>().WithMessage($"Minimum of {Order.MIN_PRODUCT_QUANTITY} units per product.");
        }

        // Update Item:
        [Fact]
        public void UpdateItem_ExistentItem_ShouldUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test product", 2, 100);
            var updatedOrderItem = new OrderItem(productId, "Test product", 5, 100);
            var newTotalValue = updatedOrderItem.GetItemTotalValue();
            order.AddItem(orderItem);

            // Act
            order.UpdateItem(updatedOrderItem);

            // Assert
            order.TotalValue.Should().Be(newTotalValue);
        }

        [Fact]
        public void UpdateItem_NonExistentItem_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test product", 2, 100);

            // Act
            Action result = () => order.UpdateItem(orderItem);

            // Assert
            result.Should().Throw<DomainException>().WithMessage($"Item {orderItem.ProductName} doesn't belong to the orders list.");
        }

        [Fact]
        public void UpdateItem_ExistentItem_ShouldUpdateItem()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test product", 2, 100);
            var updatedOrderItem = new OrderItem(productId, "Test product", 5, 100);
            var newQuantity = updatedOrderItem.Quantity;
            order.AddItem(orderItem);

            // Act
            order.UpdateItem(updatedOrderItem);

            // Assert
            order.OrderItems.FirstOrDefault(i => i.ProductId == productId).Quantity.Should().Be(newQuantity);
        }

        [Fact]
        public void UpdateItem_ItemUnitsOutOfAllowed_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test product", 5, 100);
            var updatedOrderItem1 = new OrderItem(productId, "Test product", Order.MAX_PRODUCT_QUANTITY + 1, 100);
            var updatedOrderItem2 = new OrderItem(productId, "Test product", Order.MIN_PRODUCT_QUANTITY - 1, 100);
            order.AddItem(orderItem);

            // Act
            Action result1 = () => order.UpdateItem(updatedOrderItem1);
            Action result2 = () => order.UpdateItem(updatedOrderItem2);

            // Assert
            result1.Should().Throw<DomainException>().WithMessage($"Maximum of {Order.MAX_PRODUCT_QUANTITY} units per product.");
            result2.Should().Throw<DomainException>().WithMessage($"Minimum of {Order.MIN_PRODUCT_QUANTITY} units per product.");
        }
    }
}
