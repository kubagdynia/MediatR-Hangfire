using MediatR;
using MediatRTest.Invoices.Mappers;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Queries.Handlers;

// This handler is used to get all invoices
internal sealed class GetInvoicesHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>
{
    public Task<GetInvoicesQueryResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var response = new GetInvoicesQueryResponse
        {
            // Get all invoices from the repository and convert them to Invoices
            Invoices = invoiceRepository.Get().ToInvoices()
        };

        return Task.FromResult(response);
    }
}