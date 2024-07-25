using MediatR;
using MediatRTest.Invoices.Extensions;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Queries.Handlers;

internal sealed class GetInvoicesHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>
{
    public Task<GetInvoicesQueryResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var response = new GetInvoicesQueryResponse
        {
            Invoices = invoiceRepository.Get().ToInvoices()
        };

        return Task.FromResult(response);
    }
}