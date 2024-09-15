using MediatRTest.Core.Events;

namespace MediatRTest.Invoices.Events;

// This event is used to notify that an invoice was deleted
public class InvoiceDeletedEvent : DomainEvent
{
    public required string InvoiceId { get; init; }
}