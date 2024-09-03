namespace MediatRTest.Invoices.Models;

public record Invoice
{
    public string Id { get; init; } = string.Empty;
    
    public string InvoiceNumber { get; init; } = string.Empty;
    
    public double Amount { get; init; }
    
    // This is the date the invoice was created
    public DateTime InvoiceDate { get; init; }
    
    // This is the invoice payment date
    public DateTime DueDate { get; init; }
    
    public string Currency { get; init; } = "USD";
    
    // This property is used to track if the invoice has been sent to the customer
    public bool InvoiceCreationEmailSent { get; set; }
    
    // This is the date the invoice was created
    public DateTime? CreatedAt { get; set; }
    
    public Customer Customer { get; init; } = new();
    
    public IEnumerable<InvoiceItem> Items { get; init; } = new List<InvoiceItem>();
}