using MediatR;
using MediatRTest.Invoices.Mappers;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Queries.Handlers;

// This handler is used to get an invoice
internal sealed class GetInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>
{
    public Task<GetInvoiceQueryResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var response = new GetInvoiceQueryResponse
        {
            // Get the invoice from the repository and convert it to an Invoice
            Invoice = repository.Get(request.Id)?.ToInvoice()
        };

        return Task.FromResult(response);
    }
}