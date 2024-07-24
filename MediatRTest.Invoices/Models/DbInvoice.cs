namespace MediatRTest.Invoices.Models;

public record DbInvoice(string Id, string Number, decimal Amount, DateTime CreationDate);