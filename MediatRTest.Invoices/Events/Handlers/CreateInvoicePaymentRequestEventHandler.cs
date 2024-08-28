using MediatRTest.Core.Events;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Invoices.Events.Handlers;

public class CreateInvoicePaymentRequestEventHandler(ILogger<CreateInvoicePaymentRequestEventHandler> logger) : BaseEventHandler<InvoiceCreatedEvent>
{
    protected override async Task HandleEvent(InvoiceCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting payment request: {@InvoiceId}", domainEvent.InvoiceId);

        // TODO: Implement the payment request logic here
        
        await Task.CompletedTask;
    }
}