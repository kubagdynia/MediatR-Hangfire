using MediatR;

namespace MediatRTest.Invoices.Commands;

public sealed class RemoveInvoiceCommand(string id) : IRequest<RemoveInvoiceCommandResponse>
{
    public string Id { get; } = id;
}