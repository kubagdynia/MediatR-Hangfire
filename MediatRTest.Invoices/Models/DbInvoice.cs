namespace MediatRTest.Invoices.Models;

internal record DbInvoice(string Id, string Number, decimal Amount, DateTime CreationDate);