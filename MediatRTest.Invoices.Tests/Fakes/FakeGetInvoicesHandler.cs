using MediatR;
using MediatRTest.Invoices.Queries;

namespace MediatRTest.Invoices.Tests.Fakes;

public class FakeGetInvoicesHandler(Counter counter) : IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>
{
    public Task<GetInvoicesQueryResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        counter.Increment();
        GetInvoicesQueryResponse response = new GetInvoicesQueryResponse();
        return Task.FromResult(response);
    }
}