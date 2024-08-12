using MediatR;
using MediatRTest.Data;
using Microsoft.EntityFrameworkCore;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to remove an invoice
internal sealed class RemoveInvoiceHandler(DataContext dataContext) : IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>
{
    public async Task<RemoveInvoiceCommandResponse> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Remove the invoice from the database
        var rowsDeleted = await dataContext.Invoices
            .Where(i => i.BussinsId == request.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        
        // Return the result
        RemoveInvoiceCommandResponse response = new RemoveInvoiceCommandResponse
        {
            Removed = rowsDeleted > 0
        };

        return response;
    }
}