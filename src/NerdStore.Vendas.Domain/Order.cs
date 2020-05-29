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
            if (!ItemQuantityIsValid(orderItem)) return;

            if (ItemAlreadyInList(orderItem))
            {
                var existentItem = _orderItems.FirstOrDefault(i => i.ProductId == orderItem.ProductId);
                existentItem.AddQuantity(orderItem.Quantity);
                if (!ItemQuantityIsValid(existentItem)) return;

                orderItem = existentItem;
                _orderItems.Remove(existentItem);
            }

            _orderItems.Add(orderItem);
            CalculateTotalValue();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            if (!ItemAlreadyInList(orderItem)) throw new DomainException($"Item {orderItem.ProductName} doesn't belong to the orders list.");

            if (!ItemQuantityIsValid(orderItem)) return;

            var existentItem = _orderItems.FirstOrDefault(i => i.ProductId == orderItem.ProductId);
            _orderItems.Remove(existentItem);
            _orderItems.Add(orderItem);

            CalculateTotalValue();
        }
            
        private bool ItemAlreadyInList(OrderItem orderItem)
        {
            return _orderItems.Any(i => i.ProductId == orderItem.ProductId); ;
        }

        private bool ItemQuantityIsValid(OrderItem orderItem)
        {
            var quantity = orderItem.Quantity;

            if (quantity < MIN_PRODUCT_QUANTITY || quantity > MAX_PRODUCT_QUANTITY)
            {
                var exceptionMessage = orderItem.Quantity < MIN_PRODUCT_QUANTITY ?
                    $"Minimum of {MIN_PRODUCT_QUANTITY} units per product." :
                    $"Maximum of {MAX_PRODUCT_QUANTITY} units per product.";
                throw new DomainException(exceptionMessage);
            }

            return true;
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
