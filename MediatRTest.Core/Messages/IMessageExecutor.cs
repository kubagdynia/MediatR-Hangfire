namespace MediatRTest.Core.Messages;

internal interface IMessageExecutor
{
    Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject);
    Task ExecuteEvent(MediatorSerializedObject mediatorSerializedObject);
}