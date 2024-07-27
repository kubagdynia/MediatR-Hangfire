using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

// This response is used to return the result of the CreateInvoiceCommand
public sealed class CreateInvoiceCommandResponse : BaseDomainResponse
{
    public string Id { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }

    public DateTime CreationDate { get; set; }
}