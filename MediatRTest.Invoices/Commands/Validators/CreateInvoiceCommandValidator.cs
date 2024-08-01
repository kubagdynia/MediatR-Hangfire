using FluentValidation;

namespace MediatRTest.Invoices.Commands.Validators;

// This validator is used to validate the CreateInvoiceCommand
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(c => c.Invoice).NotNull();
        RuleFor(c => c.Invoice!.InvoiceNumber).Length(5, 20);
        RuleFor(c => c.Invoice!.Amount).GreaterThan(0);
        RuleFor(c => c.Invoice!.InvoiceDate).GreaterThan(new DateTime(2024, 01, 01));//.WithMessage("{PropertyName} should be greather than {2024.01.01}");
    }
}