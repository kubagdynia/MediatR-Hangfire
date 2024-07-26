namespace MediatRTest.Core.Exceptions;

public class DomainException(string message, DomainExceptionType exceptionType = DomainExceptionType.Error) : Exception(message)
{
    public IList<DomainError> DomainErrors { get; private set; } = new List<DomainError>();
    
    public DomainExceptionType ExceptionType { get; private set; } = exceptionType;

    public void AddDomainError(string errorCode, string errorMessage, string propertyName, object attemptedValue,
        string className = "", DomainErrorType errorType = DomainErrorType.Error)
    {
        DomainErrors.Add(new DomainError(errorCode, errorMessage, propertyName, attemptedValue, className, errorType));
    }
}

public enum DomainExceptionType
{
    Error = 0,
    ValidationError,
    Conflict
}

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

public enum DomainErrorType
{
    Error = 0,
    Warning = 1,
    Info = 2
}