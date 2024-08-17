using System.Net;
using System.Net.Http.Json;
using MediatRTest.Api.Invoices.Contracts.V1;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Api.Tests;

[TestFixture]
public class ApiValidationTests
{
    private CustomWebApplicationFactory<Program> _application;
    
    [OneTimeSetUp]
    public void Init()
    {
        _application = new CustomWebApplicationFactory<Program>();
        TestHelper.CreateAndSeedDatabase(_application);
    }
    
    [OneTimeTearDown]
    public void Cleanup()
    {
        _application.Dispose();
    }

    [Test]
    public async Task CreateAnInvoiceWithoutANumberShouldResultInAnError()
    {
        // Arrange
        using var client = _application.CreateClient();

        var createInvoiceRequest = new CreateInvoiceRequest
        {
            InvoiceNumber = string.Empty,  // Empty invoice number should result in an error
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
        var response = await client.PostAsJsonAsync("/api/v1/invoices", createInvoiceRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Title.Should().Be("Validation Error");
        problemDetails.Type.Should().Be("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");
        
        List<TestHelper.Error> errors = TestHelper.DeserializeErrors(problemDetails.Extensions["errors"]);
        errors.Should().NotBeNull();
        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("LengthValidator");
        errors[0].Title.Should().Be("Validation Error");
        errors[0].UserMessage.Should().Be("'Invoice Invoice Number' must be between 5 and 20 characters. You entered 0 characters.");
    }
}