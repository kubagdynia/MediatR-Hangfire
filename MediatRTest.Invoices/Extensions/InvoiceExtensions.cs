using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Extensions;

internal static class InvoiceExtensions
{
    public static Invoice ToInvoice(this DbInvoice dbInvoice) =>
        new(dbInvoice.Id, dbInvoice.Number, dbInvoice.Amount, dbInvoice.CreationDate);
    
    public static IEnumerable<Invoice> ToInvoices(this IEnumerable<DbInvoice> dbInvoices) =>
        dbInvoices.Select(i => new Invoice(i.Id, i.Number, i.Amount, i.CreationDate)).ToList();
}