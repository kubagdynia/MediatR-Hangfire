using FluentValidation;

namespace MediatRTest.Invoices.Commands.Validators;

// This validator is used to validate the CreateInvoiceCommand before processing it in the CreateInvoiceHandler
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(c => c.Invoice).NotNull();
        RuleFor(c => c.Invoice!.InvoiceNumber).Length(5, 20);
        RuleFor(c => c.Invoice!.Amount).GreaterThan(0);
        RuleFor(c => c.Invoice!.InvoiceDate).GreaterThan(new DateTime(2024, 01, 01));
        RuleFor(c => c.Invoice!.DueDate).GreaterThan(c => c.Invoice!.InvoiceDate)
            .WithMessage("Due date should be greater than invoice date");
        RuleFor(c => c.Invoice!.Currency).Length(3);
        RuleFor(c => c.Invoice!.Customer.Name).Length(5, 50);
        RuleFor(c => c.Invoice!.Customer.Address).Length(5, 100);
        RuleFor(c => c.Invoice!.Items).NotEmpty();
        RuleFor(c => c.Invoice!.Items).Must(items => items!.All(i => i.Amount > 0))
            .WithMessage("Item amount should be greater than 0");
        RuleFor(c => c.Invoice!.Items).Must(items => items!.All(i => i.Quantity > 0))
            .WithMessage("Item quantity should be greater than 0");
        RuleFor(c => c.Invoice!.Items).Must(items => items!.All(i => i.Description.Length is > 5 and < 100))
            .WithMessage("Item description should be between 5 and 100 characters");
        RuleFor(c => c.Invoice!.Amount).Equal(c => c.Invoice!.Items.Sum(i => i.Amount * i.Quantity))
            .WithMessage("Invoice amount should be equal to the sum of the items");
    }
}