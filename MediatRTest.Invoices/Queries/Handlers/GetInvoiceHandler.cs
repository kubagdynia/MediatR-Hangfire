using MediatR;
using MediatRTest.Data;
using MediatRTest.Invoices.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MediatRTest.Invoices.Queries.Handlers;

// This handler is used to get an invoice
internal sealed class GetInvoiceHandler(DataContext dataContext) : IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>
{
    public async Task<GetInvoiceQueryResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        // Get the invoice from the database
        var invoice = await dataContext.Invoices.Include(i => i.Items)
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.BusinessId == request.Id, cancellationToken: cancellationToken);
        
        // Return the result
        var response = new GetInvoiceQueryResponse
        {
            // Get the invoice from the repository and convert it to an Invoice
            Invoice = invoice?.ToInvoice()
        };

        return response;
    }
}