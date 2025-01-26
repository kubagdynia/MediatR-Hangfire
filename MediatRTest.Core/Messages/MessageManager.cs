using System.Text.Json;
using Hangfire;
using MediatR;
using MediatRTest.Core.Configurations;
using MediatRTest.Core.Serializers;
using Microsoft.Extensions.Options;

namespace MediatRTest.Core.Messages;

internal sealed class MessageManager(IMediator mediator, IMessageExecutor messageExecutor, IOptions<HangfireConfiguration> hangfireConfig)
    : IMessageManager
{
    private readonly HangfireConfiguration _hangfireConfig = hangfireConfig.Value;

    /// <summary>
    /// Emit only once and almost immediately after creation. 
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    public async Task EmitEventAsync(INotification notification)
    {
        // If Hangfire is enabled, enqueue the job to be executed later
        if (_hangfireConfig.Enabled)
        {
            MediatorSerializedObject mediatorSerializedObject = SerializeObject(notification);
            BackgroundJob.Enqueue(() => messageExecutor.ExecuteEvent(mediatorSerializedObject));
            await Task.CompletedTask;
        }
        else
        {
            await mediator.Publish(notification);
        }
    }
    
    /// <summary>
    /// Emit only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="scheduleAt">The moment of time at which the job will be enqueued.</param>
    /// <returns>Unique identifier of a created job.</returns>
    public string EmitScheduledEvent(INotification notification, DateTimeOffset scheduleAt)
    {
        ThrowIfOperationIsNotSupported();

        MediatorSerializedObject mediatorSerializedObject = SerializeObject(notification);
        return BackgroundJob.Schedule(() => messageExecutor.ExecuteEvent(mediatorSerializedObject), scheduleAt);
    }
    
    /// <summary>
    /// Emit only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="delay">After what delay the job will be enqueued.</param>
    /// <returns>Unique identifier of a created job.</returns>
    public string EmitScheduledEvent(INotification notification, TimeSpan delay)
    {
        ThrowIfOperationIsNotSupported();

        MediatorSerializedObject mediatorSerializedObject = SerializeObject(notification);
        DateTime newTime = DateTime.Now.Add(delay);
        return BackgroundJob.Schedule(() => messageExecutor.ExecuteEvent(mediatorSerializedObject), newTime);
    }
    
    /// <summary>
    /// Emit many times on the specified CRON schedule.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="recurringJobId">Recurring job id</param>
    /// <param name="cronExpression">http://en.wikipedia.org/wiki/Cron#CRON_expression</param>
    public void EmitScheduledRecurringEvent(INotification notification, string recurringJobId, string cronExpression)
    {
        ThrowIfOperationIsNotSupported();
            
        MediatorSerializedObject mediatorSerializedObject = SerializeObject(notification);

        RecurringJob.AddOrUpdate(recurringJobId, () => messageExecutor.ExecuteEvent(mediatorSerializedObject),
            cronExpression, new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });
    }
    
    /// <summary>
    /// Execute the command only once almost immediately after creation. 
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    public async Task SendCommandAsync(IRequest request)
    {
        // If Hangfire is enabled, enqueue the job to be executed later
        if (_hangfireConfig.Enabled)
        {
            MediatorSerializedObject mediatorSerializedObject = SerializeObject(request);
            BackgroundJob.Enqueue(() => messageExecutor.ExecuteCommand(mediatorSerializedObject));
            await Task.CompletedTask;
        }
        else
        {
            await mediator.Send(request);
        }
    }
    
    /// <summary>
    /// Execute the command only once but when its parent job has been finished. 
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="parentJobId">Parent job id.</param>
    /// <param name="continuationOption">Continuation option.</param>
    /// <returns>Unique identifier of a created job.</returns>
    public string SendCommand(IRequest request, string parentJobId, JobContinuationOptions continuationOption)
    {
        ThrowIfOperationIsNotSupported();

        MediatorSerializedObject mediatorSerializedObject = SerializeObject(request);

        return BackgroundJob.ContinueJobWith(parentJobId,
            () => messageExecutor.ExecuteCommand(mediatorSerializedObject), continuationOption);
    }

    /// <summary>
    /// Execute the command only once and return the result.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <typeparam name="TResponse">Object that will be returned</typeparam>
    /// <returns>Response from the command handler</returns>
    public async Task<TResponse> SendCommandAsync<TResponse>(IRequest<TResponse> request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Execute the command only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="scheduleAt">The moment of time at which the command will be executed.</param>
    /// <returns>Unique identifier of a created job.</returns>
    public string SendScheduledCommand(IRequest request, DateTimeOffset scheduleAt)
    {
        ThrowIfOperationIsNotSupported();

        MediatorSerializedObject mediatorSerializedObject = SerializeObject(request);
        return BackgroundJob.Schedule(() => messageExecutor.ExecuteCommand(mediatorSerializedObject), scheduleAt);
    }
    
    /// <summary>
    /// Execute the command only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="delay">After what delay the command will be executed.</param>
    /// <returns>Unique identifier of a created job.</returns>
    public string SendScheduledCommand(IRequest request, TimeSpan delay)
    {
        ThrowIfOperationIsNotSupported();

        MediatorSerializedObject mediatorSerializedObject = SerializeObject(request);
        DateTime newTime = DateTime.Now.Add(delay);
        return BackgroundJob.Schedule(() => messageExecutor.ExecuteCommand(mediatorSerializedObject), newTime);
    }
    
    /// <summary>
    /// Execute the command many times on the specified CRON schedule.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="recurringJobId">Recurring job id.</param>
    /// <param name="cronExpression">http://en.wikipedia.org/wiki/Cron#CRON_expression</param>
    public void SendScheduledRecurringCommand(IRequest request, string recurringJobId, string cronExpression)
    {
        ThrowIfOperationIsNotSupported();
            
        MediatorSerializedObject mediatorSerializedObject = SerializeObject(request);

        RecurringJob.AddOrUpdate(recurringJobId, () => messageExecutor.ExecuteCommand(mediatorSerializedObject),
            cronExpression, new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });
    }
    
    private MediatorSerializedObject SerializeObject(object mediatorObject)
    {
        ArgumentNullException.ThrowIfNull(mediatorObject);
        
        string? assemblyQualifiedName = mediatorObject.GetType().AssemblyQualifiedName;

        string data = JsonSerializer.Serialize(mediatorObject, BaseJsonOptions.GetJsonSerializerOptions);

        return new MediatorSerializedObject(assemblyQualifiedName, data);
    }
    
    private void ThrowIfOperationIsNotSupported()
    {
        if (!_hangfireConfig.Enabled)
        {
            throw new NotSupportedException(
                "The operation is not supported because Hangfire is disabled. Please enable Hangfire in the configuration.");
        }
    }
}