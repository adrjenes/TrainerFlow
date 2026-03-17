namespace TrainerFlow.Modules.Offers.Features.GetOfferBySlug;

public sealed record OfferDetailsResponse(int Id, string Slug, string Name, decimal Price);