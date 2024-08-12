namespace MediatRTest.Data.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    
    public int CommandTimeout { get; set; }
    
    public bool EnableSensitiveDataLogging { get; set; }
    
    public bool EnableDetailedErrors { get; set; }
}