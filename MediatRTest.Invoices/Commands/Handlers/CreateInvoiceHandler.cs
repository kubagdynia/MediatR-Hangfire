using MediatR;
using MediatRTest.Core.Messages;
using MediatRTest.Data;
using MediatRTest.Invoices.Events;
using MediatRTest.Invoices.Mappers;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to create a new invoice
internal sealed class CreateInvoiceHandler(DataContext dataContext, IMessageManager messageManager, ILogger<CreateInvoiceHandler> logger)
    : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public async Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Generate a new invoice ID
        var invoiceId = Guid.NewGuid().ToString();
        
        logger.LogInformation("Starting creating an invoice: {@InvoiceId}", invoiceId);

        // Convert the CreateInvoiceCommand to a DbInvoice
        var invoice = request.ToDbInvoice(invoiceId);
        
        // Add the invoice to the database
        dataContext.Invoices.Add(invoice);
        await dataContext.SaveChangesAsync(cancellationToken);
        
        // Emit an event that the invoice was created
        await messageManager.EmitEventAsync(new InvoiceCreatedEvent { InvoiceId = invoice.BussinsId });
        //messageManager.EmitScheduledEvent(new InvoiceCreatedEvent { InvoiceId = invoice.BussinsId }, DateTimeOffset.Now.AddMinutes(2));
        
        // Return the invoice
        var response = new CreateInvoiceCommandResponse { Invoice = invoice.ToInvoice() };
        
        return response;
    }
}