using FluentValidation;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Api.Validators.Orders;

public sealed class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.OfferId)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}
