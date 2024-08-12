namespace MediatRTest.Data.Models;

public class Customer
{
    public int CustomerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}