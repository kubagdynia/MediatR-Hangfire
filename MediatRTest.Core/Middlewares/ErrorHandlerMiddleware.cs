using MediatRTest.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Core.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        LogException(exception);
        
        if (exception is DomainException domainException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Validation Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            };

            var errors = new List<Error>();

            foreach (var error in domainException.DomainErrors)
            {
                errors.Add(new Error {
                    Code = error.ErrorCode,
                    UserMessage = error.ErrorMessage,
                    Details = $"Validation for '{error.PropertyName}' with value '{error.AttemptedValue}' failed in {error.ClassName}",
                    Message = "Validation failed"});
            }

            if (errors.Any())
            {
                problemDetails.Extensions = new Dictionary<string, object?>
                {
                    { "errors",  errors }
                };
            }

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        else
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private void LogException(Exception exception)
        => _logger.LogError(exception, $"Exception occured: {exception.Message}");
}