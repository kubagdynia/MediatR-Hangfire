using FluentAssertions;
using MediatRTest.Core.Messages;
using MediatRTest.Data;
using MediatRTest.Invoices.Commands;
using MediatRTest.Invoices.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Tests;

[TestFixture]
public class DatabaseTests
{
    [Test]
    public async Task CanInsertInvoiceIntoDatabase()
    {
        var testConfiguration = new Dictionary<string, string?>
        {
            {"Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information"},
            {"DatabaseOptions:ConnectionString", "Data Source=sqlite.db"},
            {"DatabaseOptions:CommandTimeout", "10"},
            {"DatabaseOptions:EnableSensitiveDataLogging", "true"},
            {"DatabaseOptions:EnableDetailedErrors", "true"}
        };
        // Arrange
        ServiceProvider serviceProvider = TestHelper.PrepareServiceProvider(testConfiguration);

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        var dataContext = scopedServices.GetRequiredService<DataContext>();
        
        await dataContext.Database.EnsureDeletedAsync();
        await dataContext.Database.EnsureCreatedAsync();
        
        var messageManager = scopedServices.GetRequiredService<IMessageManager>();
        
        List<CreateInvoiceCommandResponse> createInvoiceResponses = [];

        // Act
        CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommand(
            CreateFakeCreateInvoiceCommand());

        // Assert
        createInvoiceResponse.Invoice.Should().NotBeNull();
        createInvoiceResponse.Invoice!.Id.Should().NotBeNullOrEmpty();
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