namespace MediatRTest.Api.Invoices.Contracts.V1;

public record Invoice
{
    public string InvoiceNumber { get; set; } = string.Empty;
    
    public double Amount { get; set; }
    
    // This is the date the invoice was created
    public DateTime InvoiceDate { get; set; }
    
    // This is the invoice payment date
    public DateTime DueDate { get; set; }
    
    public string Currency { get; set; } = "USD";
    
    // This property is used to track if the invoice has been sent to the customer
    public bool InvoiceCreationEmailSent { get; set; }
    
    public Customer Customer { get; set; } = new();
    
    public IEnumerable<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}