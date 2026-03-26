using FluentValidation;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Api.Validators.Orders;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Items)
            .NotNull()
            .Must(items => items is { Count: > 0 })
            .WithMessage("Order must contain at least one item.");

        When(x => x.Items is not null, () =>
        {
            RuleForEach(x => x.Items)
                .SetValidator(new CreateOrderItemCommandValidator());
        });

        When(x => x.InvoiceDetail is not null, () =>
        {
            RuleFor(x => x.InvoiceDetail!)
                .SetValidator(new CreateOrderInvoiceDetailCommandValidator());
        });
    }
}

