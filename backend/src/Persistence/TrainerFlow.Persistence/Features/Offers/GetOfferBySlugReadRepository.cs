using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;

namespace TrainerFlow.Persistence.Features.Offers;

public sealed class GetOfferBySlugReadRepository(TrainerFlowDbContext dbContext) : IGetOfferBySlugReadRepository
{
    public async Task<OfferDetailsResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await dbContext.Offers
            .AsNoTracking()
            .Where(x => x.Slug == slug)
            .Select(x => new OfferDetailsResponse(x.Id, x.Slug, x.Name, x.Price))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
