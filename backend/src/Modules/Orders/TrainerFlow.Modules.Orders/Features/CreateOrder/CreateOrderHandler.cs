using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Modules.Orders.Features.CreateOrder;

public sealed class CreateOrderHandler(IOrdersRepository ordersRepository, IOrderOfferLookup orderOfferLookup)
{
    public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        if (command.Items is null || command.Items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.", nameof(command.Items));

        var offerIds = command.Items
            .Select(x => x.OfferId)
            .Distinct()
            .ToArray();

        var availableOffers = await orderOfferLookup.GetByIdsAsync(offerIds, cancellationToken);
        var offersById = availableOffers.ToDictionary(x => x.Id);

        var order = new Order(command.Email, command.FirstName, command.LastName, command.Phone);

        if (command.InvoiceDetail is not null)
        {
            order.SetInvoiceDetail(
                command.InvoiceDetail.CompanyName,
                command.InvoiceDetail.TaxId,
                command.InvoiceDetail.Address,
                command.InvoiceDetail.City,
                command.InvoiceDetail.PostalCode,
                command.InvoiceDetail.Country);
        }

        foreach (var item in command.Items)
        {
            if (!offersById.TryGetValue(item.OfferId, out var offer))
                throw new InvalidOperationException($"Offer with id '{item.OfferId}' was not found.");

            order.AddItem(offer.Id, offer.Name, offer.Price, item.Quantity);
        }

        await ordersRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}