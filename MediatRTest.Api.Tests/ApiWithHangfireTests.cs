using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MediatRTest.Api.Invoices.Contracts.V1;

namespace MediatRTest.Api.Tests;

[TestFixture(Category = "Integration Tests")]
public class ApiWithHangfireTests
{
    private CustomWebApplicationFactory<Program> _application;
    private readonly Dictionary<string, string> _createdInvoices = new();
    
    [OneTimeSetUp]
    public void Init()
    {
        Dictionary<string, string?> testConfiguration = new Dictionary<string, string?>
        {
            { "Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information" },
            { "DatabaseOptions:ConnectionString", "Data Source=sqlite.db" },
            { "DatabaseOptions:InMemoryDatabase", "true" },
            { "DatabaseOptions:CommandTimeout", "10" },
            { "DatabaseOptions:EnableSensitiveDataLogging", "false" },
            { "DatabaseOptions:EnableDetailedErrors", "false" },
            { "Hangfire:Enabled", "true" },
            { "Hangfire:UseDashboard", "false" },
            { "Hangfire:UseInMemoryStorage", "true" },
            { "Hangfire:MaxDefaultWorkerCount", "20" }
        };
        
        _application = new CustomWebApplicationFactory<Program>(testConfiguration);
        TestHelper.CreateAndSeedDatabase(_application);
    }
    
    [OneTimeTearDown]
    public void Cleanup()
    {
        _application.Dispose();
    }
    
    [TestCase("FV/01/2024")]
    [TestCase("FV/02/2024")]
    [TestCase("FV/10/2024")]
    public async Task CanCreateAnInvoiceWithAllEvents(string invoiceNumber)
    {
        // Arrange
        using HttpClient client = _application.CreateClient();

        CreateInvoiceRequest createInvoiceRequest = new CreateInvoiceRequest
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
        };
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        InvoiceResponse? invoiceResponse = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        invoiceResponse.Should().NotBeNull();
        invoiceResponse!.Id.Should().NotBeEmpty();
        Guid.Parse(invoiceResponse.Id).Should().NotBeEmpty();
        
        invoiceResponse.InvoiceNumber.Should().Be(createInvoiceRequest.InvoiceNumber);
        invoiceResponse.Amount.Should().Be(createInvoiceRequest.Amount);
        invoiceResponse.InvoiceDate.Should().BeCloseTo(createInvoiceRequest.InvoiceDate,  TimeSpan.FromSeconds(1));
        invoiceResponse.DueDate.Should().BeCloseTo(createInvoiceRequest.DueDate,  TimeSpan.FromSeconds(1));
        invoiceResponse.Currency.Should().Be(createInvoiceRequest.Currency);
        
        invoiceResponse.Customer.Name.Should().Be(createInvoiceRequest.Customer.Name);
        invoiceResponse.Customer.Address.Should().Be(createInvoiceRequest.Customer.Address);
        
        invoiceResponse.Items.Should().HaveCount(2);
        List<InvoiceItem> requestedItems = createInvoiceRequest.Items.ToList();
        List<InvoiceItem> invoiceItems = invoiceResponse.Items.ToList();
        invoiceItems[0].Description.Should().Be(requestedItems[0].Description);
        invoiceItems[0].Amount.Should().Be(requestedItems[0].Amount);
        invoiceItems[0].Quantity.Should().Be(requestedItems[0].Quantity);
        invoiceItems[1].Description.Should().Be(requestedItems[1].Description);
        invoiceItems[1].Amount.Should().Be(requestedItems[1].Amount);
        invoiceItems[1].Quantity.Should().Be(requestedItems[1].Quantity);
        
        // The invoice creation email should not be sent immediately because is proceeded by event handler with Hangfire
        invoiceResponse.InvoiceCreationEmailSent.Should().BeFalse();
        
        // Wait for the Hangfire job to be executed
        Thread.Sleep(500);
        
        // Check if the invoice creation email was sent
        HttpResponseMessage getResponse = await client.GetAsync($"/api/v1/invoices/{invoiceResponse.Id}");
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        
        InvoiceResponse? deserializedResponse = await getResponse.Content.ReadFromJsonAsync<InvoiceResponse>();
        deserializedResponse!.InvoiceCreationEmailSent.Should().BeTrue();
        
        // Store the invoice ID and number for later use in other tests
        _createdInvoices.Add(invoiceResponse.InvoiceNumber, invoiceResponse.Id);
    }
}