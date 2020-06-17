using NerdStore.Core.Message;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class AddOrderItemEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid OrderId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public AddOrderItemEvent(Guid clientId, Guid productId, Guid orderId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = clientId;
            ProductId = productId;
            OrderId = orderId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }
    }
}
