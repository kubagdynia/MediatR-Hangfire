namespace MediatRTest.Core.Configurations;

public record HangfireConfiguration
{
    public static string SectionName = "Hangfire";

    public bool Enabled { get; init; }

    public bool UseDashboard { get; init; } = true;

    public string? ConnectionString { get; init; }
    
    public bool UseInMemoryStorage { get; init; } = true;

    public string[]? Queues { get; init; }

    public int MaxDefaultWorkerCount { get; init; }
}