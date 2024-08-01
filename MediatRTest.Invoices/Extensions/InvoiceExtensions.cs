using MediatRTest.Invoices.Commands;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Models.Db;

namespace MediatRTest.Invoices.Extensions;

internal static class InvoiceExtensions
{
    // Object mapping can also be done using, for example, the Autofac library.
    
    // This extension method is used to convert a DbInvoice to an Invoice
    public static Invoice ToInvoice(this DbInvoice dbInvoice)
        => new()
        {
            Id = dbInvoice.Id,
            InvoiceNumber = dbInvoice.InvoiceNumber,
            Amount = dbInvoice.Amount,
            InvoiceDate = dbInvoice.InvoiceDate,
            DueDate = dbInvoice.DueDate,
            Currency = dbInvoice.Currency,
            Customer = new Customer
            {
                Name = dbInvoice.Customer.Name,
                Address = dbInvoice.Customer.Address
            },
            Items = dbInvoice.Items.Select(item => new InvoiceItem
            {
                Description = item.Description,
                Amount = item.Amount,
                Quantity = item.Quantity
            })
        };
    
    // This extension method is used to convert a collection of DbInvoices to a collection of Invoices
    public static IEnumerable<Invoice> ToInvoices(this IEnumerable<DbInvoice> dbInvoices) =>
        dbInvoices.Select(i => i.ToInvoice()).ToList();
    
    // This extension method is used to convert an Invoice to a DbInvoice
    public static DbInvoice ToDbInvoice(this CreateInvoiceCommand cmd, string invoiceId)
        => new()
        {
            Id = invoiceId,
            InvoiceNumber = cmd.Invoice!.InvoiceNumber,
            Amount = cmd.Invoice.Amount,
            InvoiceDate = cmd.Invoice.InvoiceDate,
            DueDate = cmd.Invoice.DueDate,
            Currency = cmd.Invoice.Currency,
            Customer = new DbCustomer
            {
                Name = cmd.Invoice.Customer.Name,
                Address = cmd.Invoice.Customer.Address

            },
            Items = cmd.Invoice.Items.Select(item => new DbInvoiceItem
            {
                Description = item.Description,
                Amount = item.Amount,
                Quantity = item.Quantity,
            })
        };
}