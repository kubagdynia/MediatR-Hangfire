using MediatRTest.Core.Events;

namespace MediatRTest.Invoices.Events;

// This event is used to notify that an invoice was created
public class InvoiceCreatedEvent : DomainEvent
{
    public required string InvoiceId { get; init; }
}