using System;

namespace NerdStore.Vendas.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        internal decimal GetItemTotalValue()
        {
            return Quantity * UnitValue;
        }
    }
}
