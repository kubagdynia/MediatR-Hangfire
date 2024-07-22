using MediatR;

namespace MediatRTest.Core.Messages;

public interface IMessageManager
{
    void EmitEvent(INotification notification);
    
    Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> request);
}