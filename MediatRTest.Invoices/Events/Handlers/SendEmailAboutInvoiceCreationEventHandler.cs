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
        
        // Update the InvoiceCreationEmailSent flag in the database to indicate that the email has been sent
        // This is just a simple example, in a real-world scenario, you would send an email using a service like SendGrid or Mailgun
        var affected = await dataContext.Database.ExecuteSqlAsync(
            $"UPDATE Invoices SET InvoiceCreationEmailSent = 1, LastUpdated = {DateTime.Now} WHERE BussinsId = {domainEvent.InvoiceId}",
            cancellationToken);
    }
}