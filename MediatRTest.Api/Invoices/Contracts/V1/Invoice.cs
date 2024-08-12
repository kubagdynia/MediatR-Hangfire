namespace MediatRTest.Api.Invoices.Contracts.V1;

public record Invoice
{
    public string InvoiceNumber { get; init; } = string.Empty;
    
    public double Amount { get; init; }
    
    // This is the date the invoice was created
    public DateTime InvoiceDate { get; init; }
    
    // This is the invoice payment date
    public DateTime DueDate { get; init; }
    
    public string Currency { get; init; } = "USD";
    
    public Customer Customer { get; init; } = new();
    
    public IEnumerable<InvoiceItem> Items { get; init; } = new List<InvoiceItem>();
}