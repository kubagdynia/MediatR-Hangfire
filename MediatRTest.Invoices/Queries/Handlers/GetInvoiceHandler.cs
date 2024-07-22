using MediatR;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Queries.Handlers;

public class GetInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>
{
    public Task<GetInvoiceQueryResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var response = new GetInvoiceQueryResponse
        {
            Invoice = repository.Get(request.Id)
        };

        return Task.FromResult(response);
    }
}