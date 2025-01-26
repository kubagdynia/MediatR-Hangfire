using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MediatRTest.Core.Exceptions.Domain;
using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Core.Behaviors;

// Validation behavior for MediatR requests
// This behavior validates the request using FluentValidation
// If there are errors, a domain exception is thrown
// The domain exception contains the validation errors
// The validation errors are added as domain errors
// The domain errors are mapped from the FluentValidation validation failures
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseDomainResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next().ConfigureAwait(false);
        
        ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

        // Validate the request
        IEnumerable<ValidationFailure> errors = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        // If there are errors, throw a validation exception
        if (errors.Any())
        {
            ThrowValidationException(errors);
        }

        return await next().ConfigureAwait(false);
    }
    
    private static void ThrowValidationException(IEnumerable<ValidationFailure> errors)
    {
        // Create a domain exception
        DomainException exception = new DomainException("Validation Error", DomainExceptionType.ValidationError);

        // Add domain errors
        foreach (ValidationFailure error in errors)
        {
            exception.AddDomainError(error.ErrorCode, error.ErrorMessage, error.PropertyName, error.AttemptedValue,
                typeof(TRequest).Name, GetErrorType(error.Severity));
        }

        throw exception;
    }
    
    private static DomainErrorType GetErrorType(Severity severity)
    {
        // Map severity to domain error type
        DomainErrorType errorType = severity switch
        {
            Severity.Error => DomainErrorType.Error,
            Severity.Warning => DomainErrorType.Warning,
            Severity.Info => DomainErrorType.Info,
            _ => DomainErrorType.Error
        };
        return errorType;
    }
}