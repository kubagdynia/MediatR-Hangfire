using MediatRTest.Api.Invoices.Contracts.V1;
using MediatRTest.Invoices.Commands;

namespace MediatRTest.Api.Invoices.Extensions;

public static class InvoiceExtensions
{
    // Object mapping can also be done using, for example, the Autofac library.
    public static CreateInvoiceCommand ToCreateInvoiceCommand(this CreateInvoiceRequest request)
        => new()
        {
            // Mapping properties from the request to the command
            Invoice = new MediatRTest.Invoices.Models.Invoice
            {
                InvoiceNumber = request.InvoiceNumber,
                Amount = request.Amount,
                InvoiceDate = request.InvoiceDate,
                DueDate = request.DueDate,
                Currency = request.Currency,
                Customer = new MediatRTest.Invoices.Models.Customer
                {
                    Name = request.Customer.Name,
                    Address = request.Customer.Address
                },
                Items = request.Items.Select(item => new MediatRTest.Invoices.Models.InvoiceItem
                {
                    Description = item.Description,
                    Amount = item.Amount,
                    Quantity = item.Quantity,
                })
            }
        };

    public static InvoiceResponse ToInvoiceResponse(this MediatRTest.Invoices.Models.Invoice invoice)
        => new()
        {
            // Mapping properties from the invoice to the response
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            Amount = invoice.Amount,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Currency = invoice.Currency,
            Customer = new Customer
            {
                Name = invoice.Customer.Name,
                Address = invoice.Customer.Address
            },
            Items = invoice.Items.Select(item => new InvoiceItem
            {
                Description = item.Description,
                Amount = item.Amount,
                Quantity = item.Quantity,
            })
        };
}