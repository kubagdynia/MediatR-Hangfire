using MediatR;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to remove an invoice
internal sealed class RemoveInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>
{
    public Task<RemoveInvoiceCommandResponse> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
    {
        RemoveInvoiceCommandResponse response = new RemoveInvoiceCommandResponse
        {
            Removed = repository.Remove(request.Id)
        };

        return Task.FromResult(response);
    }
}