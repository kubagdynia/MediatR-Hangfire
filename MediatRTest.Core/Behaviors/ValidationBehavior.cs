using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MediatRTest.Core.Exceptions;
using MediatRTest.Core.Responses.Domain;

namespace MediatRTest.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseDomainResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next().ConfigureAwait(false);
        
        var context = new ValidationContext<TRequest>(request);

        IEnumerable<ValidationFailure> errors = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
        {
            ThrowValidationException(errors);
        }

        return await next().ConfigureAwait(false);
    }
    
    private static void ThrowValidationException(IEnumerable<ValidationFailure> errors)
    {
        var exception = new DomainException("Validation Error", DomainExceptionType.ValidationError);

        foreach (ValidationFailure error in errors)
        {
            exception.AddDomainError(error.ErrorCode, error.ErrorMessage, error.PropertyName, error.AttemptedValue,
                typeof(TRequest).Name, GetErrorType(error.Severity));
        }

        throw exception;
    }
    
    private static DomainErrorType GetErrorType(Severity severity)
    {
        var errorType = severity switch
        {
            Severity.Error => DomainErrorType.Error,
            Severity.Warning => DomainErrorType.Warning,
            Severity.Info => DomainErrorType.Info,
            _ => DomainErrorType.Error
        };
        return errorType;
    }
}