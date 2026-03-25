using FluentAssertions;
using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Domain;

public sealed class OrderInvoiceDetailTests
{
    [Fact]
    public void Constructor_ShouldCreateInvoiceDetail_WhenDataIsValid()
    {
        var invoice = new OrderInvoiceDetail("Firma XYZ", "1234567890", "Testowa 1", "Poznan", "60-001", "Polska");

        invoice.CompanyName.Should().Be("Firma XYZ");
        invoice.TaxId.Should().Be("1234567890");
        invoice.Address.Should().Be("Testowa 1");
        invoice.City.Should().Be("Poznan");
        invoice.PostalCode.Should().Be("60-001");
        invoice.Country.Should().Be("Polska");
    }

    [Fact]
    public void Constructor_ShouldTrimAllFields_WhenValuesContainSpaces()
    {
        var invoice = new OrderInvoiceDetail(
            "  Firma XYZ  ",
            "  1234567890  ",
            "  Testowa 1  ",
            "  Poznan  ",
            "  60-001  ",
            "  Polska  ");

        invoice.CompanyName.Should().Be("Firma XYZ");
        invoice.TaxId.Should().Be("1234567890");
        invoice.Address.Should().Be("Testowa 1");
        invoice.City.Should().Be("Poznan");
        invoice.PostalCode.Should().Be("60-001");
        invoice.Country.Should().Be("Polska");
    }

    [Theory]
    [InlineData("companyName")]
    [InlineData("taxId")]
    [InlineData("address")]
    [InlineData("city")]
    [InlineData("postalCode")]
    [InlineData("country")]
    public void Constructor_ShouldThrowArgumentException_WhenRequiredFieldIsEmpty(string parameterName)
    {
        Action act = parameterName switch
        {
            "companyName" => () => new OrderInvoiceDetail(" ", "123", "Adr", "City", "00-000", "PL"),
            "taxId" => () => new OrderInvoiceDetail("Firma", " ", "Adr", "City", "00-000", "PL"),
            "address" => () => new OrderInvoiceDetail("Firma", "123", " ", "City", "00-000", "PL"),
            "city" => () => new OrderInvoiceDetail("Firma", "123", "Adr", " ", "00-000", "PL"),
            "postalCode" => () => new OrderInvoiceDetail("Firma", "123", "Adr", "City", " ", "PL"),
            "country" => () => new OrderInvoiceDetail("Firma", "123", "Adr", "City", "00-000", " "),
            _ => throw new InvalidOperationException("Unsupported parameter.")
        };

        act.Should().Throw<ArgumentException>()
            .WithParameterName(parameterName);
    }
}