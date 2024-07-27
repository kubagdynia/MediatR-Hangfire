using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Extensions;

internal static class InvoiceExtensions
{
    // This extension method is used to convert a DbInvoice to an Invoice
    public static Invoice ToInvoice(this DbInvoice dbInvoice) =>
        new(dbInvoice.Id, dbInvoice.Number, dbInvoice.Amount, dbInvoice.CreationDate);
    
    // This extension method is used to convert a collection of DbInvoices to a collection of Invoices
    public static IEnumerable<Invoice> ToInvoices(this IEnumerable<DbInvoice> dbInvoices) =>
        dbInvoices.Select(i => new Invoice(i.Id, i.Number, i.Amount, i.CreationDate)).ToList();
}