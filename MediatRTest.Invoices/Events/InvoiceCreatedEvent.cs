using MediatRTest.Core.Events;

namespace MediatRTest.Invoices.Events;

public class InvoiceCreatedEvent : DomainEvent
{
    public required string InvoiceId { get; init; }
}