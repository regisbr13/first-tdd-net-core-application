using FluentAssertions;
using NerdStore.Core.Exceptions.DomainObjects;
using NerdStore.Vendas.Domain.Enums;
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
            result.Should().Throw<DomainException>().WithMessage($"Item {orderItem.ProductName} doesn't belong to the order list.");
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

        // Remove Item:
        [Fact]
        public void RemoveItem_NonExistentItem_ShouldReturnADomainException()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test product", 2, 100);

            // Act
            Action result = () => order.RemoveItem(orderItem);

            // Assert
            result.Should().Throw<DomainException>().WithMessage($"Item {orderItem.ProductName} doesn't belong to the order list.");
        }

        [Fact]
        public void RemoveItem_ExistentItem_ShouldUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test product 1", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test product 2", 1, 100);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var totalAfterRemoveItem = orderItem1.GetItemTotalValue();

            // Act
                order.RemoveItem(orderItem2);

            // Assert
            order.TotalValue.Should().Be(totalAfterRemoveItem);
        }

        [Fact]
        public void ApplyVoucher_AnyValidVoucher_ShouldReturnWithoutErrors()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO 10", null, 10, 1, true, false, DateTime.Now, VoucherType.Percentage);

            // Act
            var result = order.ApplyVoucher(voucher);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ApplyVoucher_AnyInvalidVoucher_ShouldReturnWithErrors()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var voucher = new Voucher("", null, null, 0, false, true, DateTime.Now.AddDays(-1), VoucherType.Percentage);

            // Act
            var result = order.ApplyVoucher(voucher);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ApplyVoucher_ValueVoucher_ShouldApplyDiscountOnTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO 20", 20, null, 1, true, false, DateTime.Now, VoucherType.Value);
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test product 1", 2, 50);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test product 2", 1, 100);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var valueWithDiscount = order.TotalValue - voucher.DiscountValue;

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            order.TotalValue.Should().Be(valueWithDiscount);
        }

        [Fact]
        public void ApplyVoucher_PercentageVoucher_ShouldApplyDiscountOnTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.GenerateOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO 20", null, 20, 1, true, false, DateTime.Now, VoucherType.Percentage);
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test product 1", 2, 50);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test product 2", 1, 100);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var valueWithDiscount = order.TotalValue * (1 - voucher.DiscountPercentage /  100);

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            order.TotalValue.Should().Be(valueWithDiscount);
        }
    }
}
