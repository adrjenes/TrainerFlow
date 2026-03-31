using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController(CreateOrderHandler handler, IValidator<CreateOrderCommand> validator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var orderId = await handler.HandleAsync(command, cancellationToken);

        return Ok(orderId);
    }
}

