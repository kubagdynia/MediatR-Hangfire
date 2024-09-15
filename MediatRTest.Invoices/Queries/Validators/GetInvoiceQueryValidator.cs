using FluentValidation;

namespace MediatRTest.Invoices.Queries.Validators;

public class GetInvoiceQueryValidator : AbstractValidator<GetInvoiceQuery>
{
    public GetInvoiceQueryValidator()
    {
        RuleFor(x => x.Id).Must(BeAValidGuid).WithMessage("Invalid Id.");
    }
    
    private bool BeAValidGuid(string id)
        => !string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out _);
}