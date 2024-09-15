using MediatRTest.Core.Events;

namespace MediatRTest.Invoices.Events.Handlers;

public class InvoiceDeletedEventHandler : BaseEventHandler<InvoiceDeletedEvent>
{
    protected override Task HandleEvent(InvoiceDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"--------------------> Event received. Invoice with Id:{domainEvent.InvoiceId} has been deleted.");
        
        return Task.CompletedTask;
    }
}