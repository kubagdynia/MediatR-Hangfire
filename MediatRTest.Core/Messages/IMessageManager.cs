using Hangfire;
using MediatR;

namespace MediatRTest.Core.Messages;

public interface IMessageManager
{
    /// <summary>
    /// Emit only once and almost immediately after creation. 
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    Task EmitEventAsync(INotification notification);

    /// <summary>
    /// Emit only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="scheduleAt">The moment of time at which the job will be enqueued.</param>
    /// <returns>Unique identifier of a created job.</returns>
    string EmitScheduledEvent(INotification notification, DateTimeOffset scheduleAt);

    /// <summary>
    /// Emit only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="delay">After what delay the job will be enqueued.</param>
    /// <returns>Unique identifier of a created job.</returns>
    string EmitScheduledEvent(INotification notification, TimeSpan delay);

    /// <summary>
    /// Emit many times on the specified CRON schedule.
    /// </summary>
    /// <param name="notification">Notification to be emitted.</param>
    /// <param name="recurringJobId">Recurring job id</param>
    /// <param name="cronExpression">http://en.wikipedia.org/wiki/Cron#CRON_expression</param>
    void EmitScheduledRecurringEvent(INotification notification, string recurringJobId, string cronExpression);

    /// <summary>
    /// Execute the command only once almost immediately after creation. 
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    Task SendCommandAsync(IRequest request);

    /// <summary>
    /// Execute the command only once but when its parent job has been finished. 
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="parentJobId">Parent job id.</param>
    /// <param name="continuationOption">Continuation option.</param>
    /// <returns>Unique identifier of a created job.</returns>
    string SendCommand(IRequest request, string parentJobId, JobContinuationOptions continuationOption);
    
    /// <summary>
    /// Execute the command only once and return the result.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <typeparam name="TResponse">Object that will be returned</typeparam>
    /// <returns>Response from the command handler</returns>
    Task<TResponse> SendCommandAsync<TResponse>(IRequest<TResponse> request);

    /// <summary>
    /// Execute the command only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="scheduleAt">The moment of time at which the command will be executed.</param>
    /// <returns>Unique identifier of a created job.</returns>
    string SendScheduledCommand(IRequest request, DateTimeOffset scheduleAt);

    /// <summary>
    /// Execute the command only once, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="delay">After what delay the command will be executed.</param>
    /// <returns>Unique identifier of a created job.</returns>
    string SendScheduledCommand(IRequest request, TimeSpan delay);

    /// <summary>
    /// Execute the command many times on the specified CRON schedule.
    /// </summary>
    /// <param name="request">Request to be sent.</param>
    /// <param name="recurringJobId">Recurring job id.</param>
    /// <param name="cronExpression">http://en.wikipedia.org/wiki/Cron#CRON_expression</param>
    void SendScheduledRecurringCommand(IRequest request, string recurringJobId, string cronExpression);
}