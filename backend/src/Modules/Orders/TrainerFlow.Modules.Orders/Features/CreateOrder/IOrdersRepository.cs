using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Modules.Orders.Features.CreateOrder;

public interface IOrdersRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
}
