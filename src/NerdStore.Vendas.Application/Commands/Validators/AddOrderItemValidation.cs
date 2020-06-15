using FluentValidation;
using NerdStore.Vendas.Domain;
using System;

namespace NerdStore.Vendas.Application.Commands.Validators
{
    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string ClientIdErrorMessage => "Client Id is invalid";
        public static string ProductIdErrorMessage => "Product Id is invalid";
        public static string ProductNameErrorMessage => "Product name is not informed";
        public static string QuantityMaxErrorMessage => $"The maximum quantity is {Order.MAX_PRODUCT_QUANTITY}";
        public static string QuantityMinErrorMessage => $"The minimum quantity is {Order.MIN_PRODUCT_QUANTITY}";
        public static string ValueErrorMessage => $"The item value must be greater than 0";

        public AddOrderItemValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErrorMessage);

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMessage);

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage(ProductNameErrorMessage);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityMinErrorMessage)
                .LessThanOrEqualTo(Order.MAX_PRODUCT_QUANTITY)
                .WithMessage(QuantityMaxErrorMessage);
            
            RuleFor(x => x.UnitValue)
                .GreaterThan(0)
                .WithMessage(ValueErrorMessage);
        }
    }
}
