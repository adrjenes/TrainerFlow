using TrainerFlow.Modules.Offers.Domain;

namespace TrainerFlow.Persistence.Seeding;

internal static class OfferSeed
{
    public static readonly Offer[] Data =
    [
        new(
            slug: "indywidualny-plan-treningowy",
            name: "Indywidualny Plan Treningowy",
            price: 189.00m),

        new(
            slug: "indywidualny-plan-dietetyczny",
            name: "Indywidualny Plan Dietetyczny",
            price: 169.00m),

        new(
            slug: "pakiet-trening-dieta",
            name: "Pakiet: Plan Treningowy + Dietetyczny",
            price: 299.00m)
    ];
}