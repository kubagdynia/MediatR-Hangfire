using MediatR;
using MediatRTest.Invoices.Commands;

namespace MediatRTest.Invoices.Tests.Fakes;

public class FakeRemoveInvoiceHandler(Counter counter)
    : IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>
{
    public Task<RemoveInvoiceCommandResponse> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
    {
        counter.Increment();
        var response = new RemoveInvoiceCommandResponse { Removed = true };
        return Task.FromResult(response);
    }
}