namespace TrainerFlow.Modules.Offers.Features.GetOffers;

public sealed record OfferResponse(int Id, string Slug, string Name, decimal Price);