using MediatR;

namespace MediatRTest.Core.Events;

public abstract class BaseEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    /// <summary>
    /// Handles domain event.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleEvent(domainEvent, cancellationToken);
    }
    
    /// <summary>
    /// Actual implementation of event handling.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected abstract Task HandleEvent(TDomainEvent domainEvent, CancellationToken cancellationToken);
}