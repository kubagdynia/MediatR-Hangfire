namespace MediatRTest.Core.Exceptions.Domain;

// Exception used to handle domain exceptions
public class DomainException(string message, DomainExceptionType exceptionType = DomainExceptionType.Error) : Exception(message)
{
    public IList<DomainError> DomainErrors { get; } = new List<DomainError>();
    
    public DomainExceptionType ExceptionType { get; private set; } = exceptionType;

    public void AddDomainError(string errorCode, string errorMessage, string propertyName, object attemptedValue,
        string className = "", DomainErrorType errorType = DomainErrorType.Error)
    {
        DomainErrors.Add(new DomainError(errorCode, errorMessage, propertyName, attemptedValue, className, errorType));
    }
}