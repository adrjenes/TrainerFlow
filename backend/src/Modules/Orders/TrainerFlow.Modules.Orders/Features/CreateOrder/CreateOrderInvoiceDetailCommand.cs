namespace TrainerFlow.Modules.Orders.Features.CreateOrder;

public sealed record CreateOrderInvoiceDetailCommand(
    string CompanyName,
    string TaxId,
    string Address,
    string City,
    string PostalCode,
    string Country
);