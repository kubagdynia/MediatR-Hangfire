namespace MediatRTest.Core.Middlewares;

public record Error
{
    public string? Message { get; set; }
    public string? Code { get; set; }
    public string? Details { get; set; }
    public string? UserMessage { get; set; }
}