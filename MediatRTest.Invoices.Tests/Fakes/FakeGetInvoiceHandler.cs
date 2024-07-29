using MediatR;
using MediatRTest.Invoices.Queries;

namespace MediatRTest.Invoices.Tests.Fakes;

public class FakeGetInvoiceHandler(Counter counter) : IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>
{
    public Task<GetInvoiceQueryResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        counter.Increment();
        var response = new GetInvoiceQueryResponse();
        return Task.FromResult(response);
    }
}