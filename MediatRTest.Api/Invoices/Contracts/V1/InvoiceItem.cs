namespace MediatRTest.Api.Invoices.Contracts.V1;

public record InvoiceItem
{
    public string Description { get; init; } = string.Empty;
    
    public decimal Amount { get; init; }
    
    public int Quantity { get; init; }
    
    public decimal Total => Amount * Quantity;
}