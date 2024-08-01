using MediatR;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Commands;

// This command is used to create a new invoice
public sealed class CreateInvoiceCommand : IRequest<CreateInvoiceCommandResponse>
{
    public Invoice? Invoice { get; set; }
}