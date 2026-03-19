namespace TrainerFlow.Modules.Offers.Features.GetOffers;

public sealed class GetOffersHandler(IGetOffersReadRepository repository)
{
    public async Task<IReadOnlyList<OfferResponse>> HandleAsync(GetOffersQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}