using FluentAssertions;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Tests;

[TestFixture]
public class DatabaseTests
{
    [Test]
    public async Task CanInsertInvoiceIntoDatabaseAndGetItBack()
    {
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        // Act
        // Create a new invoice and add it to the database
        CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommand(
            CreateFakeCreateInvoiceCommand());
        
        string invoiceId = createInvoiceResponse.Invoice!.Id;
        
        // Get the invoice from the database using the ID
        GetInvoiceQueryResponse queryResponse =
            await messageManager.SendCommand(new GetInvoiceQuery(createInvoiceResponse.Invoice!.Id));
        
        // Assert
        queryResponse.Invoice.Should().NotBeNull();
        invoiceId.Should().Be(queryResponse.Invoice!.Id);
    }

    private CreateInvoiceCommand CreateFakeCreateInvoiceCommand(string invoiceNumber = "FV/01/2024")
        => new()
        {
            Invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                Amount = 790.11,
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Currency = "USD",
                Customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main St"

                },
                Items = new List<InvoiceItem>
                {
                    new()
                    {
                        Description = "Item 1",
                        Amount = 123.45,
                        Quantity = 2
                    },
                    new()
                    {
                        Description = "Item 2",
                        Amount = 543.21,
                        Quantity = 1
                    }
                }
            }
        };
}