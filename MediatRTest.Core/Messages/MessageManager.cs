using MediatR;

namespace MediatRTest.Core.Messages;

internal sealed class MessageManager(IMediator mediator, IPublisher publisher) : IMessageManager
{
    /// <summary>
    /// Emit only once and almost immediately after creation. 
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    public async void EmitEvent(INotification notification)
    {
        await publisher.Publish(notification);
    }

    /// <summary>
    /// Execute the command only once and return the result.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <typeparam name="TResponse">Object that will be returned</typeparam>
    /// <returns>Response from the command handler</returns>
    public async Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> request)
    {
        var response = await mediator.Send(request);
        return response;
    }
}