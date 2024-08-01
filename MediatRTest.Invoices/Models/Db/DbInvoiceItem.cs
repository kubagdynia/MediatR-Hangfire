namespace MediatRTest.Invoices.Models.Db;

public record DbInvoiceItem
{
    public string Description { get; init; } = string.Empty;
    
    public decimal Amount { get; init; }
    
    public int Quantity { get; init; }
}