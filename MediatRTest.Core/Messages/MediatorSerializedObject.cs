namespace MediatRTest.Core.Messages;

internal class MediatorSerializedObject(string? assemblyQualifiedName, string data)
{
    public string? AssemblyQualifiedName { get; private set; } = assemblyQualifiedName;

    public string Data { get; private set; } = data;

    /// <summary>
    /// Override for Hangfire dashboard display
    /// </summary>
    public override string ToString()
    {
        var fullTypeName = GetFullTypeName();
        return fullTypeName is null ? "No command name" : fullTypeName.Split('.').Last();
    }
    
    private string? GetFullTypeName()
    {
        if (AssemblyQualifiedName is null)
        {
            return null;
        }
        
        string? fullTypeName = null;
        
        var parts = AssemblyQualifiedName.Split(',');
        if (parts.Length > 0)
        {
            fullTypeName = parts[0];
        }

        return fullTypeName;
    }
}