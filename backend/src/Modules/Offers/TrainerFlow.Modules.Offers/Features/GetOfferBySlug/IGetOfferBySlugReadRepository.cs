namespace TrainerFlow.Modules.Offers.Features.GetOfferBySlug;

public interface IGetOfferBySlugReadRepository
{
    Task<OfferDetailsResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}
