using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Persistence.Features.Offers;

public sealed class GetOffersReadRepository(TrainerFlowDbContext dbContext) : IGetOffersReadRepository
{
    public async Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Offers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new OfferResponse(x.Id, x.Slug, x.Name, x.Price))
            .ToListAsync(cancellationToken);
    }
}
