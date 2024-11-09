using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

// This response is used to return the result of the RemoveInvoiceCommand
public sealed record RemoveInvoiceCommandResponse : BaseDomainResponse
{
    public bool Removed { get; set; }
}