using MediatR;

namespace MediatRTest.Invoices.Commands;

// This command is used to create a new invoice
public sealed class CreateInvoiceCommand(string number, decimal amount, DateTime creationDate) : IRequest<CreateInvoiceCommandResponse>
{
    public string Number { get; set; } = number;

    public decimal Amount { get; set; } = amount;

    public DateTime CreationDate { get; set; } = creationDate;
}