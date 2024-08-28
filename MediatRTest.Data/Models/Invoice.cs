namespace MediatRTest.Data.Models;

public class Invoice
{
    public int InvoiceId { get; set; }
    
    public string BussinsId { get; set; } = string.Empty;
    
    public string InvoiceNumber { get; set; } = string.Empty;
    
    public double Amount { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public string Currency { get; set; } = string.Empty;

    // This property is used to track if the invoice has been sent to the customer
    public bool InvoiceCreationEmailSent { get; set; } = false;
    
    public int CustomerId { get; set; } // Required foreign key to principal 'Customer'
    public Customer Customer { get; set; } = null!; // Required reference navigation to principal 'Customer
    
    public List<InvoiceItem> Items { get; set; } = []; // Collection navigation to related 'InvoiceItem' entities
}