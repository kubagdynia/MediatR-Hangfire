namespace MediatRTest.Invoices.Models;

public class Invoice
{
    public string Id { get; set; }
    public string Number { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreationDate { get; set; }

    public Invoice(string id, string number, decimal amount, DateTime creationDate)
    {
        Id = id;
        Number = number;
        Amount = amount;
        CreationDate = creationDate;
    }
}