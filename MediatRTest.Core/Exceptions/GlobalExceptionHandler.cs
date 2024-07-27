using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Core.Exceptions;

internal sealed record Error(string? Code, string? Title, string? Details, string? UserMessage);

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"Exception occured: {exception.Message}");
        
        if (exception is DomainException domainException)
        {
            // Handle domain exceptions
            var problemDetails = domainException.ExceptionType switch
            {
                DomainExceptionType.ValidationError => ExecuteValidationError(httpContext, domainException),
                DomainExceptionType.Error => ExecuteConflict(httpContext, domainException),
                DomainExceptionType.Conflict => ExecuteConflict(httpContext, domainException),
                _ => throw new ArgumentOutOfRangeException()
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
        else
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };
            
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        return true;
    }
    
    // Execute validation error
    private static ProblemDetails ExecuteValidationError(HttpContext httpContext, DomainException domainException)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = GetProblemDetails(
            domainException,
            title: "Validation Error",
            problemStatus: StatusCodes.Status400BadRequest,
            problemType: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            detailMessage: error => $"Validation for '{error.PropertyName}' with value '{error.AttemptedValue}' failed in {error.ClassName}");

        return problemDetails;
    }
    
    // Execute conflict
    private static ProblemDetails ExecuteConflict(HttpContext httpContext, DomainException domainException)
    {
        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        
        var problemDetails = GetProblemDetails(
            domainException,
            title: "Error",
            problemStatus: StatusCodes.Status409Conflict,
            problemType: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            detailMessage: null);

        return problemDetails;
    }

    private static ProblemDetails GetProblemDetails(DomainException domainException,
        string title, int problemStatus, string problemType,
        Func<DomainError, string>? detailMessage = null)
    {
        var problemDetails = new ProblemDetails
        {
            Status = problemStatus,
            Title = title,
            Type = problemType
        };
        
        var errors = new List<Error>();
        
        foreach (var error in domainException.DomainErrors)
        {
            errors.Add(new Error(
                Code: error.ErrorCode,
                Title: title,
                UserMessage: error.ErrorMessage,
                Details: detailMessage?.Invoke(error)
            ));
        }
        
        if (errors.Any())
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                { "errors", errors }
            };
        }
        
        return problemDetails;
    }
}