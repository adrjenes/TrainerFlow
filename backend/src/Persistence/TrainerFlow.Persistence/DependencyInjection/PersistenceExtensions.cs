using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;
using TrainerFlow.Modules.Offers.Features.GetOffers;
using TrainerFlow.Persistence.Features.Offers;

namespace TrainerFlow.Persistence.DependencyInjection;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TrainerFlowDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseTrainerFlowSeeding());

        services.AddScoped<IGetOffersReadRepository, GetOffersReadRepository>();
        services.AddScoped<IGetOfferBySlugReadRepository, GetOfferBySlugReadRepository>();

        return services;
    }
}