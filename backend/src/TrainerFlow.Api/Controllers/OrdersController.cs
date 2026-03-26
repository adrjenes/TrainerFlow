using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TrainerFlow.Api.Common.Validation;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController(CreateOrderHandler handler, IValidator<CreateOrderCommand> validator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var orderId = await handler.HandleAsync(command, cancellationToken);

        return Ok(orderId);
    }
}

