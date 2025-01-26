using MediatR;
using MediatRTest.Invoices.Commands;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Tests.Fakes;

public class FakeCreateInvoiceHandler(Counter counter)
    : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        counter.Increment();
        CreateInvoiceCommandResponse result = 
            new CreateInvoiceCommandResponse { Invoice = new Invoice { Id = Guid.NewGuid().ToString() } };
        return Task.FromResult(result);

    }
}