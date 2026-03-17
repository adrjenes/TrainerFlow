using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;
using TrainerFlow.Modules.Offers.Features.GetOffers;

public interface IOffersRepository
{
    Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<OfferDetailsResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}
