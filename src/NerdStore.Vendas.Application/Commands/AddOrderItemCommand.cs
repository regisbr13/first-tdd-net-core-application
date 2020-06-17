using NerdStore.Core.Message;
using NerdStore.Vendas.Application.Commands.Validators;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }

        public AddOrderItemCommand(Guid clientId, Guid productId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = clientId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
