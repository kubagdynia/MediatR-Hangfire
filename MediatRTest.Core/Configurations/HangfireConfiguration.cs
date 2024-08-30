namespace MediatRTest.Core.Configurations;

public class HangfireConfiguration
{
    public static string SectionName = "Hangfire";

    public bool Enabled { get; set; } = false;

    public bool UseDashboard { get; set; } = true;

    public string? ConnectionString { get; set; }
    
    public bool UseInMemoryStorage { get; set; } = true;

    public string[]? Queues { get; set; }

    public int MaxDefaultWorkerCount { get; set; }
}