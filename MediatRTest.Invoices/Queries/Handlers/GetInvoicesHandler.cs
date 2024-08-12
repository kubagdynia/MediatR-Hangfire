using MediatR;
using MediatRTest.Data;
using MediatRTest.Invoices.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MediatRTest.Invoices.Queries.Handlers;

// This handler is used to get all invoices
internal sealed class GetInvoicesHandler(DataContext dataContext) : IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>
{
    public async Task<GetInvoicesQueryResponse> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        // Get all invoices from the database
        var invoices = await dataContext.Invoices.Include(i => i.Items)
            .Include(c => c.Customer).ToListAsync(cancellationToken: cancellationToken);
        
        // Return the result
        var response = new GetInvoicesQueryResponse
        {
            // Get all invoices from the repository and convert them to Invoices
            Invoices = invoices.ToInvoices()
        };

        return response;
    }
}