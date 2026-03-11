using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Domain;
using TrainerFlow.Persistence.Seeding;

namespace TrainerFlow.Persistence;

public static class TrainerFlowDbContextExtensions // https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
{
    public static DbContextOptionsBuilder UseTrainerFlowSeeding(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            var offers = context.Set<Offer>();

            if (offers.Any())
            {
                return;
            }
            offers.AddRange(OfferSeed.Data);
            context.SaveChanges();
        });
        return optionsBuilder;
    }
}