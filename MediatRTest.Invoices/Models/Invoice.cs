namespace MediatRTest.Invoices.Models;

public record Invoice(string Id, string Number, decimal Amount, DateTime CreationDate);