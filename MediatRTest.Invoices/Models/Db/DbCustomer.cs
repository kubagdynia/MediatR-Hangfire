namespace MediatRTest.Invoices.Models.Db;

public record DbCustomer
{
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
};