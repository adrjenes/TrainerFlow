using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;
using TrainerFlow.Modules.Offers.Features.GetOffers;


namespace TrainerFlow.Persistence.Features.Offers;

public sealed class OffersRepository(TrainerFlowDbContext dbContext) : IOffersRepository
{
    public async Task<IReadOnlyList<OfferResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var offers = await dbContext.Offers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new OfferResponse(x.Id, x.Slug, x.Name, x.Price))
            .ToListAsync(cancellationToken);

        return offers;
    }

    public async Task<OfferDetailsResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await dbContext.Offers
            .AsNoTracking()
            .Where(x => x.Slug == slug)
            .Select(x => new OfferDetailsResponse(x.Id, x.Slug, x.Name, x.Price))
            .SingleOrDefaultAsync(cancellationToken);
    }
}