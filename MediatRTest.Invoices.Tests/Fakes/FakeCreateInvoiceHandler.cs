using MediatR;
using MediatRTest.Invoices.Commands;

namespace MediatRTest.Invoices.Tests.Fakes;

public class FakeCreateInvoiceHandler(Counter counter)
    : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        counter.Increment();
        var result = new CreateInvoiceCommandResponse { Id = Guid.NewGuid().ToString() };
        return Task.FromResult(result);

    }
}