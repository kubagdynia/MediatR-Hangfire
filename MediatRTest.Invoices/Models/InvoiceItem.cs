namespace MediatRTest.Invoices.Models;

public record InvoiceItem
{
    public string Description { get; init; } = string.Empty;
    
    public decimal Amount { get; init; }
    
    public int Quantity { get; init; }
    
    public decimal Total => Amount * Quantity;
}