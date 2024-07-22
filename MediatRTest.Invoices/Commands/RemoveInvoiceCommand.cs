using MediatR;

namespace MediatRTest.Invoices.Commands;

public class RemoveInvoiceCommand(string id) : IRequest<RemoveInvoiceCommandResponse>
{
    public string Id { get; } = id;
}