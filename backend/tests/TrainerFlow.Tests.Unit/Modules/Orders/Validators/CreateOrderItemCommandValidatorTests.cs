using FluentValidation.TestHelper;
using TrainerFlow.Api.Validators.Orders;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Validators;

public sealed class CreateOrderItemCommandValidatorTests
{
    private readonly CreateOrderItemCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_OfferId_Is_Not_Greater_Than_Zero()
    {
        var model = new CreateOrderItemCommand(0, 1);

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.OfferId);
    }

    [Fact]
    public void Should_Have_Error_When_Quantity_Is_Not_Greater_Than_Zero()
    {
        var model = new CreateOrderItemCommand(1, 0);

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        var model = new CreateOrderItemCommand(1, 2);

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}