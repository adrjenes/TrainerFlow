using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Persistence.Features.Orders;

public sealed class OrderOfferLookup(TrainerFlowDbContext dbContext) : IOrderOfferLookup
{
    public async Task<IReadOnlyList<OrderOfferDto>> GetByIdsAsync(IReadOnlyCollection<int> offerIds, CancellationToken cancellationToken)
    {
        if (offerIds.Count == 0) return [];
        
        return await dbContext.Offers
            .AsNoTracking()
            .Where(x => offerIds.Contains(x.Id))
            .Select(x => new OrderOfferDto(x.Id, x.Name, x.Price))
            .ToListAsync(cancellationToken);
    }
}

