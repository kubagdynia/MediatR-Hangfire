using MediatR.Pipeline;

namespace MediatRTest.Core.Logging;

internal sealed class LoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingPostProcessor<TRequest, TResponse>> _logger;

    public LoggingPostProcessor(ILogger<LoggingPostProcessor<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processed {RequestName} with response {Response}", typeof(TRequest).Name, response);
        return Task.CompletedTask;
    }
}