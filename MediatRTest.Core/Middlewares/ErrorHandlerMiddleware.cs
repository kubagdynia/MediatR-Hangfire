using MediatRTest.Core.Exceptions.Domain;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Core.Middlewares;

internal sealed class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
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
            
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Validation Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            };

            List<Error> errors = [];

            foreach (DomainError error in domainException.DomainErrors)
            {
                errors.Add(new Error(
                    Code: error.ErrorCode,
                    Title: "Validation failed",
                    UserMessage: error.ErrorMessage,
                    Details: $"Validation for '{error.PropertyName}' with value '{error.AttemptedValue}' failed in {error.ClassName}"
                    ));
            }

            if (errors.Count != 0)
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
            ProblemDetails problemDetails = new ProblemDetails
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
        => logger.LogError(exception, "Exception occured: {Message}", exception.Message);
}

internal sealed record Error(string? Code, string? Title, string? Details, string? UserMessage);