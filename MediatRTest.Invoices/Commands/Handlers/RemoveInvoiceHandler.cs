using MediatR;
using MediatRTest.Core.Messages;
using MediatRTest.Data;
using MediatRTest.Invoices.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to remove an invoice
internal sealed class RemoveInvoiceHandler(DataContext dataContext, IMessageManager messageManager, ILogger<RemoveInvoiceHandler> logger)
    : IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>
{
    public async Task<RemoveInvoiceCommandResponse> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting removing an invoice: {@InvoiceId}", request.Id);
        
        // Remove the invoice from the database
        var rowsDeleted = await dataContext.Invoices
            .Where(i => i.BussinsId == request.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (rowsDeleted == 0)
        {
            return new RemoveInvoiceCommandResponse
            {
                Removed = false
            };
        }

        // Emit an event that the invoice was deleted
        messageManager.EmitEvent(new InvoiceDeletedEvent { InvoiceId = request.Id });

        // Return the result
        RemoveInvoiceCommandResponse response = new RemoveInvoiceCommandResponse
        {
            Removed = true
        };

        return response;
    }
}