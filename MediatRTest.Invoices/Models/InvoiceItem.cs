namespace MediatRTest.Invoices.Models;

public record InvoiceItem
{
    public string Description { get; init; } = string.Empty;
    
    public double Amount { get; init; }
    
    public int Quantity { get; init; }
    
    public double Total => Amount * Quantity;
}