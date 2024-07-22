using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

public class RemoveInvoiceCommandResponse : BaseDomainResponse
{
    public bool Removed { get; set; }
}