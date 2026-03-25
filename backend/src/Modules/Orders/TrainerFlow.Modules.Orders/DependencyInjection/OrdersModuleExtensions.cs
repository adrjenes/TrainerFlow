using Microsoft.Extensions.DependencyInjection;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Modules.Orders.DependencyInjection;

public static class OrdersModuleExtensions
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        return services;
    }
}