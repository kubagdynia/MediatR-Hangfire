using MediatR;

namespace MediatRTest.Invoices.Commands;

// This command is used to remove an invoice
public sealed class RemoveInvoiceCommand(string id) : IRequest<RemoveInvoiceCommandResponse>
{
    public string Id { get; } = id;
}