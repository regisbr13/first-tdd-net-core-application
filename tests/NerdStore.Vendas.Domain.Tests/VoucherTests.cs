using FluentAssertions;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Validators;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact]
        public void Validate_ValidValueVoucher_ShouldReturnTrue()
        {
            // Arrange
            var voucher = new Voucher("PROMO 10", 10, null, 1, true, false, DateTime.Now, VoucherType.Value);
            
            // Act
            var result = voucher.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_InValidValueVoucher_ShouldReturnFalse()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, false, true, DateTime.Now.AddDays(-1), VoucherType.Value);

            // Act
            var result = voucher.Validate();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(6);
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.CodeErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.DiscountValueErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.QuantityErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.ValidationDateErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.UsedErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.ActiveErrorMessage)).Should().BeTrue();
        }

        [Fact]
        public void Validate_ValidPercentageVoucher_ShouldReturnTrue()
        {
            // Arrange
            var voucher = new Voucher("PROMO 10", null, 10, 1, true, false, DateTime.Now, VoucherType.Percentage);

            // Act
            var result = voucher.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_InValidPercentageVoucher_ShouldReturnFalse()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, false, true, DateTime.Now.AddDays(-1), VoucherType.Percentage);

            // Act
            var result = voucher.Validate();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(6);
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.CodeErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.DiscountPercentageErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.QuantityErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.ValidationDateErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.UsedErrorMessage)).Should().BeTrue();
            result.Errors.Any(e => e.ErrorMessage.Equals(VoucherValidation.ActiveErrorMessage)).Should().BeTrue();
        }
    }
}
