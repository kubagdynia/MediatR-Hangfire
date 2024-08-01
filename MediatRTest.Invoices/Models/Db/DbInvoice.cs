namespace MediatRTest.Invoices.Models.Db;

internal record DbInvoice
{
    public string Id { get; init; } = string.Empty;
    
    public string InvoiceNumber { get; init; } = string.Empty;
    
    public decimal Amount { get; init; }
    
    public DateTime InvoiceDate { get; init; }
    
    public DateTime DueDate { get; init; }
    
    public string Currency { get; init; } = string.Empty;
    
    public DbCustomer Customer { get; init; } = new();
    
    public IEnumerable<DbInvoiceItem> Items { get; init; } = new List<DbInvoiceItem>();
}