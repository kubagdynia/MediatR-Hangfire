namespace MediatRTest.Data.Models;

public class InvoiceItem
{
    public int InvoiceItemId { get; set; }
    
    public string Description { get; init; } = string.Empty;
    
    public double Amount { get; init; }
    
    public int Quantity { get; init; }
    
    public int InvoiceId { get; set; } // Required foreign key to principal 'Invoice'
}