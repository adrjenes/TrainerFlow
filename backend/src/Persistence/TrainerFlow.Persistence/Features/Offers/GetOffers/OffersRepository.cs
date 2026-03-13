using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Persistence.Features.Offers.GetOffers;

public sealed class OffersReadRepository(TrainerFlowDbContext dbContext) : IOffersReadRepository
{
    public async Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<OfferResponse> offers = await dbContext.Offers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new OfferResponse(
                x.Id,
                x.Slug,
                x.Name,
                x.Price))
            .ToListAsync(cancellationToken);

        return offers;
    }
}