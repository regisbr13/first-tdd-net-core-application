using NerdStore.Core.Exceptions.DomainObjects;
using NerdStore.Vendas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Order
    {
        private readonly List<OrderItem> _orderItems;
        public const int MIN_PRODUCT_QUANTITY = 1;
        public const int MAX_PRODUCT_QUANTITY = 15;
        public decimal TotalValue { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public OrderStatus OrderStatus { get; set; }
        public Guid CustomerId { get; set; }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem.Quantity < MIN_PRODUCT_QUANTITY || orderItem.Quantity > MAX_PRODUCT_QUANTITY)
            {
                var exceptionMessage = orderItem.Quantity < MIN_PRODUCT_QUANTITY ? 
                    $"Minimum of {MIN_PRODUCT_QUANTITY} units for product." : 
                    $"Maximum of {MAX_PRODUCT_QUANTITY} units for product.";
                throw new DomainException(exceptionMessage);
            }

            var inList = _orderItems.Any(i => i.ProductId == orderItem.ProductId);
            if (inList)
            {
                var existentItem = _orderItems.FirstOrDefault(i => i.ProductId == orderItem.ProductId);
                existentItem.AddQuantity(orderItem.Quantity);
                orderItem = existentItem;
                _orderItems.Remove(existentItem);
            }

            _orderItems.Add(orderItem);
            CalculateTotalValue();
        }

        public void MakeDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        private void CalculateTotalValue()
        {
            TotalValue = _orderItems.Sum(i => i.GetItemTotalValue());
        }

        public static class OrderFactory
        {
            public static Order GenerateOrder(Guid customerId)
            {
                var order = new Order { CustomerId = customerId };
                order.MakeDraft();
                return order;
            }
        }
    }

}
