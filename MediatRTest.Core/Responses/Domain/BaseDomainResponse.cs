namespace MediatRTest.Core.Responses.Domain;

public class BaseDomainResponse
{
    public IList<string>? Errors { get; private set; }

    public void AddError(string errorMessage)
    {
        Errors ??= new List<string>();
        Errors.Add(errorMessage);
    }
}