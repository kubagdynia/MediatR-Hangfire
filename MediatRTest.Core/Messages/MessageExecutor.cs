using System.Text.Json;
using MediatR;
using MediatRTest.Core.Serializers;

namespace MediatRTest.Core.Messages;

internal class MessageExecutor(IMediator mediator) : IMessageExecutor
{
    // Asynchronously send a request to a single handler with no response
    public Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject)
        => mediator.Send(GetMessage<IRequest>(mediatorSerializedObject));

    // Asynchronously send a notification to multiple handlers
    public Task ExecuteEvent(MediatorSerializedObject mediatorSerializedObject)
        => mediator.Publish(GetMessage<INotification>(mediatorSerializedObject));
    
    private static T GetMessage<T>(MediatorSerializedObject mediatorSerializedObject) where T : class
    {
        ArgumentNullException.ThrowIfNull(mediatorSerializedObject);
        ArgumentNullException.ThrowIfNull(mediatorSerializedObject.AssemblyQualifiedName);

        // Get the type of the mediator serialized object
        Type? type = Type.GetType(mediatorSerializedObject.AssemblyQualifiedName);

        ArgumentNullException.ThrowIfNull(type);

        // Deserialize the mediator serialized object
        if (JsonSerializer.Deserialize(mediatorSerializedObject.Data, type, BaseJsonOptions.GetJsonSerializerOptions) is not T notification)
        {
            throw new InvalidCastException(nameof(notification));
        }

        return notification;
    }
}