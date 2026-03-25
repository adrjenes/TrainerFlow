using TrainerFlow.Modules.Orders.Domain;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Persistence.Features.Orders;

public sealed class OrdersRepository(TrainerFlowDbContext dbContext) : IOrdersRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}