using System.Net;
using System.Net.Http.Json;
using MediatRTest.Api.Invoices.Contracts.V1;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Api.Tests;

[TestFixture(Category = "Integration Tests")]
public class ApiValidationTests
{
    private CustomWebApplicationFactory<Program> _application;
    
    [OneTimeSetUp]
    public void Init()
    {
        var testConfiguration = new Dictionary<string, string?>
        {
            { "Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information" },
            { "DatabaseOptions:ConnectionString", "Data Source=sqlite.db" },
            { "DatabaseOptions:InMemoryDatabase", "true" },
            { "DatabaseOptions:CommandTimeout", "10" },
            { "DatabaseOptions:EnableSensitiveDataLogging", "false" },
            { "DatabaseOptions:EnableDetailedErrors", "false" }
        };
        
        _application = new CustomWebApplicationFactory<Program>(testConfiguration);
        TestHelper.CreateAndSeedDatabase(_application);
    }
    
    [OneTimeTearDown]
    public void Cleanup()
    {
        _application.Dispose();
    }

    [Test]
    public async Task CannotCreateAnInvoiceWithAnEmptyInvoiceNumber()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.InvoiceNumber = string.Empty;  // Empty invoice number should result in an error
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("LengthValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("'Invoice Invoice Number' must be between 5 and 20 characters. You entered 0 characters.");
    }

    [Test]
    public async Task CannotCreateAnInvoiceWithAnAmountEqualToZero()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 0; // Amount should be greater than 0, so it should result in an error
            request.Items = new List<InvoiceItem>
            {
                new()
                {
                    Description = "Item 1",
                    Amount = 0, // Amount should be greater than 0, so it should result in an error
                    Quantity = 2
                }
            };
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(2);
        
        errors[0].Code.Should().Be("GreaterThanValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("'Invoice Amount' must be greater than '0'.");
        
        errors[1].Code.Should().Be("PredicateValidator");
        errors[1].Title.Should().Be("Validation Error");
        errors[1].UserMessage.Should().Be("Item amount should be greater than 0.");
    }
    
    [Test]
    public async Task InvoiceDueDateShouldBeGreaterThanInvoiceDate()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 790.11;
            request.InvoiceDate = DateTime.Now;
            request.DueDate = DateTime.Now.AddDays(-1); // Due date should be greater than invoice date
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("GreaterThanValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Due date should be greater than invoice date.");
    }
    
    [Test]
    public async Task InvoiceCurrencyShouldConsistOfThreeCharacters()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Currency = "US"; // Currency should consist of three characters
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("ExactLengthValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("'Invoice Currency' must be 3 characters in length. You entered 2 characters.");
    }
    
    [Test]
    public async Task InvoiceItemsShouldNotBeEmpty()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Items = new List<InvoiceItem>();
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(2);
        
        errors[0].Code.Should().Be("NotEmptyValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("'Invoice Items' must not be empty.");
        
        errors[1].Code.Should().Be("EqualValidator");
        errors[1].Title.Should().Be("Validation Error");
        errors[1].UserMessage.Should().Be("Invoice amount should be equal to the sum of the items.");
    }
    
    [Test]
    public async Task InvoiceItemsAmountShouldBeGreaterThanZero()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 100;
            request.Items = new List<InvoiceItem>
            {
                new()
                {
                    Description = "Item 1",
                    Amount = 100,
                    Quantity = 1
                },
                new()
                {
                    Description = "Item 2",
                    Amount = 0,
                    Quantity = 1
                }
            };
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("PredicateValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Item amount should be greater than 0.");
    }
    
    [Test]
    public async Task InvoiceItemsQuantityShouldBeGreaterThanZero()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 100;
            request.Items = new List<InvoiceItem>
            {
                new()
                {
                    Description = "Item 1",
                    Amount = 100,
                    Quantity = 1
                },
                new()
                {
                    Description = "Item 2",
                    Amount = 10,
                    Quantity = 0
                }
            };
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("PredicateValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Item quantity should be greater than 0.");
    }
    
    [Test]
    public async Task InvoiceItemsDescriptionShouldBeBetween5And100Characters()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 100;
            request.Items = new List<InvoiceItem>
            {
                new()
                {
                    Description = "",
                    Amount = 90,
                    Quantity = 1
                },
                new()
                {
                    Description = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                    Amount = 10,
                    Quantity = 1
                }
            };
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("PredicateValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Item description should be between 5 and 100 characters.");
    }
    
    [Test]
    public async Task InvoiceAmountShouldBeEqualToTheSumOfTheItems()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.Amount = 150;
            request.Items = new List<InvoiceItem>
            {
                new()
                {
                    Description = "Item 1",
                    Amount = 50,
                    Quantity = 2
                },
                new()
                {
                    Description = "Item 2",
                    Amount = 20,
                    Quantity = 3
                }
            };
        });
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("EqualValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Invoice amount should be equal to the sum of the items.");
    }
    
    [TestCase("test")]
    [TestCase("   123  ")]
    public async Task AttemptingToRetrieveAnInvoiceWithAnInvalidIdShouldReturnAValidationError(string invoiceId)
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        // Act
        HttpResponseMessage response = await client.GetAsync($"/api/v1/invoices/{invoiceId}");
        
        // Assert
        List<TestHelper.Error> errors = await CheckValidationError(response);
        errors.Should().HaveCount(1);
        
        errors[0].Code.Should().Be("PredicateValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("Invalid Id.");
    }
    
    [Test]
    public async Task AttemptingToRetrieveAnInvoiceWithAnValidIdShouldReturnAnInvoice()
    {
        // Arrange
        using HttpClient client = _application.CreateClient();
        
        var createInvoiceRequest = CreateInvoiceRequest(request =>
        {
            request.InvoiceNumber = "FV/153/2024";
        });
        
        // Act
        HttpResponseMessage createInvoiceResponse = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        createInvoiceResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Wait for the invoice to be created
        Thread.Sleep(200);
        
        if (createInvoiceResponse.IsSuccessStatusCode)
        {
            InvoiceResponse? invoiceResponse = await createInvoiceResponse.Content.ReadFromJsonAsync<InvoiceResponse>();
            invoiceResponse.Should().NotBeNull();
            string invoiceId = invoiceResponse!.Id;
            
            // Act
            HttpResponseMessage response = await client.GetAsync($"/api/v1/invoices/{invoiceId}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var retrievedInvoiceResponse = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
            retrievedInvoiceResponse.Should().NotBeNull();
            retrievedInvoiceResponse!.Id.Should().Be(invoiceId);
            retrievedInvoiceResponse.InvoiceNumber.Should().Be(createInvoiceRequest.InvoiceNumber);
            retrievedInvoiceResponse.Amount.Should().Be(createInvoiceRequest.Amount);
            retrievedInvoiceResponse.InvoiceDate.Should().BeCloseTo(createInvoiceRequest.InvoiceDate, TimeSpan.FromSeconds(1));
            retrievedInvoiceResponse.DueDate.Should().BeCloseTo(createInvoiceRequest.DueDate, TimeSpan.FromSeconds(1));
            retrievedInvoiceResponse.Currency.Should().Be(createInvoiceRequest.Currency);
            retrievedInvoiceResponse.Customer.Name.Should().Be(createInvoiceRequest.Customer.Name);
            retrievedInvoiceResponse.Customer.Address.Should().Be(createInvoiceRequest.Customer.Address);
            retrievedInvoiceResponse.Items.Should().HaveCount(2);
        }
    }

    private static CreateInvoiceRequest CreateInvoiceRequest(Action<CreateInvoiceRequest> createInvoiceRequestAction)
    {
        var createInvoiceRequest = new CreateInvoiceRequest
        {
            InvoiceNumber = "FV/01/2024",
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
        
        createInvoiceRequestAction(createInvoiceRequest);
        
        return createInvoiceRequest;
    }
    
    private static async Task<List<TestHelper.Error>> CheckValidationError(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Title.Should().Be("Validation Error");
        problemDetails.Type.Should().Be("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");
        
        List<TestHelper.Error> errors = TestHelper.DeserializeErrors(problemDetails.Extensions["errors"]);
        errors.Should().NotBeNull();
        return errors;
    }
}