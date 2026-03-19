namespace TrainerFlow.Modules.Offers.Features.GetOffers;

public interface IGetOffersReadRepository
{
    Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken);
}
