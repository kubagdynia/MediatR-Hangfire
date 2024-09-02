namespace MediatRTest.Core.Messages;

internal interface IMessageExecutor
{
    // Asynchronously send a request to a single handler with no response
    Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject);
    
    // Asynchronously send a notification to multiple handlers
    Task ExecuteEvent(MediatorSerializedObject mediatorSerializedObject);
}