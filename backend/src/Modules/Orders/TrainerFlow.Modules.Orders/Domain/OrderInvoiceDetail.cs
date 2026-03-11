namespace TrainerFlow.Modules.Orders.Domain;

public sealed class OrderInvoiceDetail
{
    public string CompanyName { get; private set; } = null!;
    public string TaxId { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;
    public string Country { get; private set; } = null!;

    private OrderInvoiceDetail() { }

    public OrderInvoiceDetail(string companyName, string taxId, string address, string city, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(companyName)) throw new ArgumentException("Company name is required.", nameof(companyName));

        if (string.IsNullOrWhiteSpace(taxId)) throw new ArgumentException("Tax ID is required.", nameof(taxId));

        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required.", nameof(address));

        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));

        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("Postal code is required.", nameof(postalCode));

        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required.", nameof(country));

        CompanyName = companyName.Trim();
        TaxId = taxId.Trim();
        Address = address.Trim();
        City = city.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
    }
}
