using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Invoices.Commands;

public class CreateInvoiceCommandResponse(string id, string number, DateTime creationDate) : BaseDomainResponse
{
    public string Id { get; set; } = id;

    public string Number { get; set; } = number;

    public DateTime CreationDate { get; set; } = creationDate;
}