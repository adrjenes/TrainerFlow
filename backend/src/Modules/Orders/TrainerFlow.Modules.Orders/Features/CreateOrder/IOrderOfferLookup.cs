namespace TrainerFlow.Modules.Orders.Features.CreateOrder;

public interface IOrderOfferLookup
{
    Task<IReadOnlyList<OrderOfferDto>> GetByIdsAsync(IReadOnlyCollection<int> offerIds, CancellationToken cancellationToken);
}