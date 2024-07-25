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
        var exception = new DomainException("Validation Error");

        foreach (ValidationFailure error in errors)
        {
            exception.AddDomainError(error.ErrorCode, error.ErrorMessage, error.PropertyName, error.AttemptedValue,
                typeof(TRequest).Name, DomainErrorType.ValidationError);
        }

        throw exception;
    }
}