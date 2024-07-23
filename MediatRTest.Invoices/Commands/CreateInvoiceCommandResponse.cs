using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

public class CreateInvoiceCommandResponse : BaseDomainResponse
{
    public string Id { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }
}