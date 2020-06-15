using FluentAssertions;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Commands.Validators;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests
{
    public class AddOrderItemCommandTests
    {
        [Fact]
        public void AddOrderItemCommand_ValidCommand_ShouldPassWithoutErrors()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 50);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AddOrderItemCommand_InvalidCommand_ShouldShowValidationErrors()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            result.Should().BeFalse();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.ClientIdErrorMessage)).Should().BeTrue();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.ProductIdErrorMessage)).Should().BeTrue();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.ProductNameErrorMessage)).Should().BeTrue();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.QuantityMinErrorMessage)).Should().BeTrue();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.ValueErrorMessage)).Should().BeTrue();
        }


        [Fact]
        public void AddOrderItemCommand_CommandWithQuantityAboveAllowed_ShouldShowValidationErrors()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test product", Order.MAX_PRODUCT_QUANTITY + 1, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            result.Should().BeFalse();
            orderCommand.ValidationResult.Errors.Any(e => e.ErrorMessage.Equals(AddOrderItemValidation.QuantityMaxErrorMessage)).Should().BeTrue();
        }
    }
}
