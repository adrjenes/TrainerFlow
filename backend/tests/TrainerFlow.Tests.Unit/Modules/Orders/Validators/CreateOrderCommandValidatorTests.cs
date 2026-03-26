using FluentAssertions;
using FluentValidation.TestHelper;
using TrainerFlow.Api.Validators.Orders;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Validators;

public sealed class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = CreateValidModel() with { Email = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = CreateValidModel() with { Email = "invalid-email" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Items_Are_Empty()
    {
        var model = CreateValidModel() with { Items = [] };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact]
    public void Should_Have_Error_When_Item_Is_Invalid()
    {
        var model = CreateValidModel() with
        {
            Items = [new CreateOrderItemCommand(0, 0)]
        };

        var result = _validator.TestValidate(model);

        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Have_Error_When_InvoiceDetail_Is_Invalid()
    {
        var model = CreateValidModel() with
        {
            InvoiceDetail = new CreateOrderInvoiceDetailCommand(
                "",
                "1234567890",
                "Testowa 1",
                "Poznan",
                "60-001",
                "Polska")
        };

        var result = _validator.TestValidate(model);

        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        var model = CreateValidModel();

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateOrderCommand CreateValidModel() =>
        new(
            "test@example.com",
            "Jan",
            "Kowalski",
            "123456789",
            [new CreateOrderItemCommand(1, 1)],
            new CreateOrderInvoiceDetailCommand(
                "Firma XYZ",
                "1234567890",
                "Testowa 1",
                "Poznan",
                "60-001",
                "Polska"));
}