namespace MediatRTest.Invoices.Models;

public class Invoice
{
    public string Id { get; set; }
    public string Number { get; set; }
    public DateTime CreationDate { get; set; }

    public Invoice()
    {

    }

    public Invoice(string id, string number, DateTime creationDate)
    {
        Id = id;
        Number = number;
        CreationDate = creationDate;
    }
}