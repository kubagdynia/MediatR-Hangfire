namespace MediatRTest.Core.Exceptions.Domain;

public class DomainError(
    string errorCode,
    string errorMessage,
    string propertyName,
    object attemptedValue,
    string className,
    DomainErrorType errorType = DomainErrorType.Error)
{
    public string ErrorCode { get; } = errorCode;
    
    public string ErrorMessage { get; } = errorMessage;
    
    public string PropertyName { get; } = propertyName;
    
    public object AttemptedValue { get; } = attemptedValue;
    
    public DomainErrorType ErrorType { get; } = errorType;
    
    public string ClassName { get; } = className;
}