using MediatR;

namespace MediatRTest.Core.Messages;

public class MessageManager(IMediator mediator) : IMessageManager
{
    /// <summary>
    /// Emit only once and almost immediately after creation. 
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    public void EmitEvent(INotification notification)
    {
        mediator.Publish(notification);
    }

    /// <summary>
    /// Execute the command only once and return the result.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <typeparam name="TResponse">Object that will be returned</typeparam>
    /// <returns>Response from the command handler</returns>
    public Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> request)
    {
        var response = mediator.Send(request);
        return response;
    }
}