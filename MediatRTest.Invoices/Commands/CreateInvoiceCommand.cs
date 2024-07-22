using MediatR;

namespace MediatRTest.Invoices.Commands;

public class CreateInvoiceCommand(string id, string number, DateTime creationDate) : IRequest<CreateInvoiceCommandResponse>
{
    public string Id { get; set; } = id;

    public string Number { get; set; } = number;

    public DateTime CreationDate { get; set; } = creationDate;
}