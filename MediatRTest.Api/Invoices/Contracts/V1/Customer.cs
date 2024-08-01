namespace MediatRTest.Api.Invoices.Contracts.V1;

public record Customer
{
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
}