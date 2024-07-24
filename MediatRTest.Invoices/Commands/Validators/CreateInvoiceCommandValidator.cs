using FluentValidation;

namespace MediatRTest.Invoices.Commands.Validators;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(c => c.Number).Length(5, 20);
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.CreationDate).GreaterThan(new DateTime(2024, 01, 01));//.WithMessage("{PropertyName} should be greather than {2024.01.01}");
    }
}