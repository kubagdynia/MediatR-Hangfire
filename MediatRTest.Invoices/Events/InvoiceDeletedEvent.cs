using MediatRTest.Core.Events;

namespace MediatRTest.Invoices.Events;

public class InvoiceDeletedEvent : DomainEvent
{
    public required string InvoiceId { get; init; }
}