namespace MediatRTest.Core.Responses.Domain;

public class BaseDomainResponse
{
    public IList<string>? Errors { get; private set; }

    public void AddError(string errorMessage)
    {
        // Add error message to the list of errors
        Errors ??= new List<string>();
        Errors.Add(errorMessage);
    }
}