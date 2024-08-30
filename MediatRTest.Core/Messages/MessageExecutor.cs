using System.Text.Json;
using MediatR;
using MediatRTest.Core.Serializers;

namespace MediatRTest.Core.Messages;

internal class MessageExecutor(IMediator mediator) : IMessageExecutor
{
    public Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject)
        => mediator.Send(GetMessage<IRequest>(mediatorSerializedObject));

    public Task ExecuteEvent(MediatorSerializedObject mediatorSerializedObject)
        => mediator.Publish(GetMessage<INotification>(mediatorSerializedObject));

    private static T GetMessage<T>(MediatorSerializedObject mediatorSerializedObject) where T : class
    {
        ArgumentNullException.ThrowIfNull(mediatorSerializedObject);
        ArgumentNullException.ThrowIfNull(mediatorSerializedObject.AssemblyQualifiedName);

        var type = Type.GetType(mediatorSerializedObject.AssemblyQualifiedName);

        ArgumentNullException.ThrowIfNull(type);

        if (JsonSerializer.Deserialize(mediatorSerializedObject.Data, type, BaseJsonOptions.GetJsonSerializerOptions) is not T notification)
        {
            throw new InvalidCastException(nameof(notification));
        }

        return notification;
    }
}