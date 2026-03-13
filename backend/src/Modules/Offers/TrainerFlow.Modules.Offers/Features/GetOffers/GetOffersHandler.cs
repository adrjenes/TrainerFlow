namespace TrainerFlow.Modules.Offers.Features.GetOffers;

public sealed class GetOffersHandler(IOffersReadRepository offersReadRepository)
{
    public async Task<IReadOnlyList<OfferResponse>> HandleAsync(GetOffersQuery query, CancellationToken cancellationToken)
    {
        return await offersReadRepository.GetAllAsync(cancellationToken);
    }
}