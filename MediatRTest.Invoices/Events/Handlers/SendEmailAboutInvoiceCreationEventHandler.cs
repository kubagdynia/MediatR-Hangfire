using MediatRTest.Core.Events;
using MediatRTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Invoices.Events.Handlers;

internal class SendEmailAboutInvoiceCreationEventHandler(DataContext dataContext,
    ILogger<SendEmailAboutInvoiceCreationEventHandler> logger) : BaseEventHandler<InvoiceCreatedEvent>
{
    protected override async Task HandleEvent(InvoiceCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending an email about invoice creation: {@InvoiceId}", domainEvent.InvoiceId);

        // TODO: Implement the email sending logic here
        // For now, we will just sleep for 200 milliseconds to simulate the email sending process
        Thread.Sleep(100);
        
        // Set the InvoiceCreationEmailSent flag to true and save the changes to the database
        var affected = await dataContext.Invoices
            .Where(c => c.BussinsId == domainEvent.InvoiceId)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.InvoiceCreationEmailSent, true), cancellationToken);
    }
}