using FluentValidation.TestHelper;
using TrainerFlow.Api.Validators.Orders;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Validators;

public sealed class CreateOrderInvoiceDetailCommandValidatorTests
{
    private readonly CreateOrderInvoiceDetailCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_CompanyName_Is_Empty()
    {
        var model = CreateValidModel() with { CompanyName = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
    }

    [Fact]
    public void Should_Have_Error_When_TaxId_Is_Empty()
    {
        var model = CreateValidModel() with { TaxId = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.TaxId);
    }

    [Fact]
    public void Should_Have_Error_When_Address_Is_Empty()
    {
        var model = CreateValidModel() with { Address = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Should_Have_Error_When_City_Is_Empty()
    {
        var model = CreateValidModel() with { City = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void Should_Have_Error_When_PostalCode_Is_Empty()
    {
        var model = CreateValidModel() with { PostalCode = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }

    [Fact]
    public void Should_Have_Error_When_Country_Is_Empty()
    {
        var model = CreateValidModel() with { Country = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        var model = CreateValidModel();

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateOrderInvoiceDetailCommand CreateValidModel() =>
        new(
            "Firma XYZ",
            "1234567890",
            "Testowa 1",
            "Poznan",
            "60-001",
            "Polska");
}
