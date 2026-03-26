using FluentValidation;
using TrainerFlow.Modules.Orders.Features.CreateOrder;

namespace TrainerFlow.Api.Validators.Orders;

public sealed class CreateOrderInvoiceDetailCommandValidator : AbstractValidator<CreateOrderInvoiceDetailCommand>
{
    public CreateOrderInvoiceDetailCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.TaxId)
            .NotEmpty()
            .MaximumLength(12);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(70);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(6);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}