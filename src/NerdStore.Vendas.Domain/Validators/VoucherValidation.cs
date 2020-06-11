using FluentValidation;
using NerdStore.Vendas.Domain.Enums;
using System;

namespace NerdStore.Vendas.Domain.Validators
{
    public class VoucherValidation : AbstractValidator<Voucher>
    {
        public static string CodeErrorMessage => "Voucher doesn't have a valid code";
        public static string ValidationDateErrorMessage => "This Voucher is expired";
        public static string ActiveErrorMessage => "This Voucher isn't active";
        public static string UsedErrorMessage => "This Voucher has already been used";
        public static string QuantityErrorMessage => "This Voucher isn't available";
        public static string DiscountValueErrorMessage => "The discount value must be greater than 0";
        public static string DiscountPercentageErrorMessage => "The discount percentage  must be greater than 0";

        public VoucherValidation()
        {
            RuleFor(v => v.Code).NotEmpty().WithMessage(CodeErrorMessage);
            RuleFor(v => v.ValidationDate).GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage(ValidationDateErrorMessage);
            RuleFor(v => v.Active).Equal(true).WithMessage(ActiveErrorMessage);
            RuleFor(v => v.Used).Equal(false).WithMessage(UsedErrorMessage);
            RuleFor(v => v.Quantity).GreaterThan(0).WithMessage(QuantityErrorMessage);

            When(v => v.VoucherType == VoucherType.Value, () =>
            {
                RuleFor(v => v.DiscountValue).NotNull().WithMessage(DiscountValueErrorMessage)
                .GreaterThan(0).WithMessage(DiscountValueErrorMessage);
            });

            When(v => v.VoucherType == VoucherType.Percentage, () =>
            {
                RuleFor(v => v.DiscountPercentage).NotNull().WithMessage(DiscountPercentageErrorMessage)
                .GreaterThan(0).WithMessage(DiscountPercentageErrorMessage);
            });
        }
    }
}
