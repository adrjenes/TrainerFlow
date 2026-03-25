namespace TrainerFlow.Modules.Orders.Features.CreateOrder;

public sealed record CreateOrderCommand(
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    IReadOnlyList<CreateOrderItemCommand> Items,
    CreateOrderInvoiceDetailCommand? InvoiceDetail
);