using MediatR;

namespace MediatRTest.Invoices.Commands;

public class CreateInvoiceCommand(string number, DateTime creationDate) : IRequest<CreateInvoiceCommandResponse>
{
    public string Number { get; set; } = number;

    public DateTime CreationDate { get; set; } = creationDate;
}