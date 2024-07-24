using MediatR;

namespace MediatRTest.Invoices.Commands;

public class CreateInvoiceCommand(string number, decimal amount, DateTime creationDate) : IRequest<CreateInvoiceCommandResponse>
{
    public string Number { get; set; } = number;

    public decimal Amount { get; set; } = amount;

    public DateTime CreationDate { get; set; } = creationDate;
}