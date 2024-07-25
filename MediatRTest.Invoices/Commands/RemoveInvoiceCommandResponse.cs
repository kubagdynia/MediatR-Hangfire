using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

public sealed class RemoveInvoiceCommandResponse : BaseDomainResponse
{
    public bool Removed { get; set; }
}