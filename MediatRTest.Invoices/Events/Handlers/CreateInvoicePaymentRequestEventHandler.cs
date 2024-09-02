using MediatRTest.Core.Events;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Invoices.Events.Handlers;

public class CreateInvoicePaymentRequestEventHandler(ILogger<CreateInvoicePaymentRequestEventHandler> logger) : BaseEventHandler<InvoiceCreatedEvent>
{
    protected override async Task HandleEvent(InvoiceCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting payment request: {@InvoiceId}", domainEvent.InvoiceId);

        // TODO: Implement the payment request logic here
        // For now, we will just sleep for 100 milliseconds to simulate the payment request process
        Thread.Sleep(100);
        
        await Task.CompletedTask;
    }
}