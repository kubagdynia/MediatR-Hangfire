using MediatR.Pipeline;

namespace MediatRTest.Core.Logging;

// Class is used to log the request and response of the MediatR pipeline
internal sealed class LoggingPostProcessor<TRequest, TResponse>(
    ILogger<LoggingPostProcessor<TRequest, TResponse>> logger) : IRequestPostProcessor<TRequest, TResponse> where TRequest : notnull
{
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        // Log the request and response
        logger.LogInformation("[MediatR] Processed {RequestName} with response {Response}", typeof(TRequest).Name, response);
        return Task.CompletedTask;
    }
}