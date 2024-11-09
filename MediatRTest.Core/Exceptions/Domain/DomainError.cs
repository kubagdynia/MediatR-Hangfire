namespace MediatRTest.Core.Exceptions.Domain;

public record DomainError(
    string ErrorCode,
    string ErrorMessage,
    string PropertyName,
    object AttemptedValue,
    string ClassName,
    DomainErrorType ErrorType = DomainErrorType.Error);