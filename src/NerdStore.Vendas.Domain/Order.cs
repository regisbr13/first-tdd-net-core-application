using NerdStore.Vendas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Order
    {
        private readonly List<OrderItem> _orderItems;
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

        public void CalculateTotalValue()
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
