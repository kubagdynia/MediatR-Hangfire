using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Core.Exceptions;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Create(HttpContext httpContext, DomainException domainException)
    {
        // Handle domain exceptions
        var problemDetails = domainException.ExceptionType switch
        {
            DomainExceptionType.ValidationError => ExecuteValidationError(httpContext, domainException),
            DomainExceptionType.Error => ExecuteConflict(httpContext, domainException),
            DomainExceptionType.Conflict => ExecuteConflict(httpContext, domainException),
            _ => throw new ArgumentOutOfRangeException()
        };

        return problemDetails;
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
            detailMessage: null);
            //detailMessage: error => $"Validation for '{error.PropertyName}' with value '{error.AttemptedValue}' failed in {error.ClassName}");

        return problemDetails;
    }
    
    // Execute conflict error
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

    // Get problem details from domain exception
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
        
        // Add domain errors to the problem details
        foreach (var error in domainException.DomainErrors)
        {
            errors.Add(new Error(
                Code: error.ErrorCode,
                Title: title,
                UserMessage: error.ErrorMessage,
                Details: detailMessage?.Invoke(error)
            ));
        }
        
        // Add errors to the problem details extensions
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