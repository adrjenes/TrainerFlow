using Microsoft.AspNetCore.Mvc;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OffersController(GetOffersHandler getOffersHandler, GetOfferBySlugHandler getOfferBySlugHandler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OfferResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var offers = await getOffersHandler.HandleAsync(new GetOffersQuery(), cancellationToken);
        return Ok(offers);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<OfferDetailsResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var offer = await getOfferBySlugHandler.HandleAsync(new GetOfferBySlugQuery(slug), cancellationToken);
        if (offer is null) return NotFound();
        return Ok(offer);
    }
}