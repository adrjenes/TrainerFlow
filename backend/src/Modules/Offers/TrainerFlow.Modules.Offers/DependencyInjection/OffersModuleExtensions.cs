using Microsoft.Extensions.DependencyInjection;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Modules.Offers.DependencyInjection;

public static class OffersModuleExtensions
{
    public static IServiceCollection AddOffersModule(this IServiceCollection services)
    {
        services.AddScoped<GetOffersHandler>();
        services.AddScoped<GetOfferBySlugHandler>();

        return services;
    }
}