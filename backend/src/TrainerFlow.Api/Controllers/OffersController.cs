using Microsoft.AspNetCore.Mvc;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OffersController(GetOffersHandler getOffersHandler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OfferResponse>>> GetAll(CancellationToken cT)
    {
        var offers = await getOffersHandler.HandleAsync(new GetOffersQuery(), cT);
        return Ok(offers);
    }
}