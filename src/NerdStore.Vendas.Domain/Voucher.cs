using FluentValidation.Results;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Validators;
using System;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public string Code { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public int Quantity { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }
        public DateTime ValidationDate { get; private set; }
        public VoucherType VoucherType { get; private set; }

        public Voucher(string code, decimal? discountValue, decimal? discountPercentage, int quantity, bool active, bool used, DateTime validationDate, VoucherType voucherType)
        {
            Code = code;
            DiscountValue = discountValue;
            DiscountPercentage = discountPercentage;
            Quantity = quantity;
            Active = active;
            Used = used;
            ValidationDate = validationDate;
            VoucherType = voucherType;
        }

        public ValidationResult Validate()
        {
            return new VoucherValidation().Validate(this);
        }
    }
}
