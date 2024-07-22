using MediatR;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Queries.Handlers;

public class GetInvoicesHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>
{
    public Task<GetInvoicesQueryResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var response = new GetInvoicesQueryResponse
        {
            Invoices = invoiceRepository.Get()
        };

        return Task.FromResult(response);
    }
}