using MediatRTest.Invoices.Commands;
using Customer = MediatRTest.Data.Models.Customer;
using Invoice = MediatRTest.Data.Models.Invoice;
using InvoiceItem = MediatRTest.Data.Models.InvoiceItem;

namespace MediatRTest.Invoices.Mappers;

internal static class InvoiceExtensions
{
    // Object mapping can also be done using, for example, the Autofac library.
    
    // This extension method is used to convert a DbInvoice to an Invoice
    public static Models.Invoice ToInvoice(this Invoice invoice)
        => new()
        {
            Id = invoice.BusinessId,
            InvoiceNumber = invoice.InvoiceNumber,
            Amount = invoice.Amount,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Currency = invoice.Currency,
            InvoiceCreationEmailSent = invoice.InvoiceCreationEmailSent,
            CreatedAt = invoice.CreatedAt,
            Customer = new Models.Customer
            {
                Name = invoice.Customer.Name,
                Address = invoice.Customer.Address
            },
            Items = invoice.Items.Select(item => new Models.InvoiceItem
            {
                Description = item.Description,
                Amount = item.Amount,
                Quantity = item.Quantity
            })
        };
    
    // This extension method is used to convert a collection of DbInvoices to a collection of Invoices
    public static IEnumerable<Models.Invoice> ToInvoices(this IEnumerable<Invoice> dbInvoices) =>
        dbInvoices.Select(i => i.ToInvoice()).ToList();
    
    // This extension method is used to convert an Invoice to a DbInvoice
    public static Invoice ToDbInvoice(this CreateInvoiceCommand cmd, string invoiceId)
        => new()
        {
            BusinessId = invoiceId,
            InvoiceNumber = cmd.Invoice!.InvoiceNumber,
            Amount = cmd.Invoice.Amount,
            InvoiceDate = cmd.Invoice.InvoiceDate,
            DueDate = cmd.Invoice.DueDate,
            Currency = cmd.Invoice.Currency,
            Customer = new Customer
            {
                Name = cmd.Invoice.Customer.Name,
                Address = cmd.Invoice.Customer.Address

            },
            Items = cmd.Invoice.Items.Select(item => new InvoiceItem
            {
                Description = item.Description,
                Amount = item.Amount,
                Quantity = item.Quantity,
            }).ToList()
        };
}