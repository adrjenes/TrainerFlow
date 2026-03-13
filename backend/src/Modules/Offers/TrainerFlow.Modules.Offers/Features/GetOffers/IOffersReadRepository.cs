namespace TrainerFlow.Modules.Offers.Features.GetOffers;

public interface IOffersReadRepository
{
    Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken);
}