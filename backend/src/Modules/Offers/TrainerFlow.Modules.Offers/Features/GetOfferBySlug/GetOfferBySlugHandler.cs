namespace TrainerFlow.Modules.Offers.Features.GetOfferBySlug;

public sealed class GetOfferBySlugHandler(IOffersRepository repository)
{
    public async Task<OfferDetailsResponse?> HandleAsync(GetOfferBySlugQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.Slug)) return null;

        string normalizedSlug = query.Slug.Trim().ToLowerInvariant();

        return await repository.GetBySlugAsync(normalizedSlug, cancellationToken);
    }
}