using FluentAssertions;
using MediatR;
using MediatRTest.Core.Exceptions;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Queries;
using Microsoft.Extensions.DependencyInjection;
using MediatRTest.Invoices.Tests.Fakes;

namespace MediatRTest.Invoices.Tests;

[TestFixture]
public class InvoiceOperationsTests
{
    [TestCase(1)]
    [TestCase(5)]
    public async Task All_created_invoices_should_be_added_to_the_repository(int count)
    {
        // Arrange
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        List<CreateInvoiceCommandResponse> createInvoiceResponses = [];

        // Act
        for (var i = 0; i < count; i++)
        {
            CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommandAsync(
                CreateFakeCreateInvoiceCommand());
            createInvoiceResponses.Add(createInvoiceResponse);
        }

        GetInvoicesQueryResponse queryResponse = await messageManager.SendCommandAsync(new GetInvoicesQuery());

        // Assert
        queryResponse.Invoices.Should().HaveCount(count);
        queryResponse.Invoices.Should().NotBeNullOrEmpty();

        // checking if the invoices ids match the created invoices ids
        for (var i = 0; i < queryResponse.Invoices?.Count(); i++)
        {
            var repoInvoiceId = queryResponse.Invoices.ElementAt(i).Id;
            var createdInvoiceId = createInvoiceResponses[i].Invoice?.Id;

            repoInvoiceId.Should().BeEquivalentTo(createdInvoiceId);
        }
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task All_created_invoices_should_be_able_to_get_by_passing_their_id(int count)
    {
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();
        
        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        // Arrange

        var createInvoiceResponses = new List<CreateInvoiceCommandResponse>();

        // Act
        for (var i = 0; i < count; i++)
        {
            CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommandAsync(
                CreateFakeCreateInvoiceCommand());
            createInvoiceResponses.Add(createInvoiceResponse);
        }

        // Assert
        foreach (var createdInvoice in createInvoiceResponses)
        {
            GetInvoiceQueryResponse queryResponse =
                await messageManager.SendCommandAsync(new GetInvoiceQuery(createdInvoice.Invoice?.Id!));
            queryResponse.Invoice.Should().NotBeNull();
            queryResponse.Invoice?.Id.Should().BeEquivalentTo(createdInvoice.Invoice?.Id);
        }
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task Id_should_be_possible_to_delete_all_created_invoices(int count)
    {
        // Arrange
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();

        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        var createInvoiceResponses = new List<CreateInvoiceCommandResponse>();

        // Act
        for (var i = 0; i < count; i++)
        {
            CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommandAsync(
                CreateFakeCreateInvoiceCommand());
            createInvoiceResponses.Add(createInvoiceResponse);
        }

        // Assert
        foreach (var createdInvoice in createInvoiceResponses)
        {
            RemoveInvoiceCommandResponse removeResponse = await messageManager.SendCommandAsync(new RemoveInvoiceCommand(createdInvoice.Invoice?.Id!));
            removeResponse.Removed.Should().BeTrue();
        }

        // Repo should be empty after removing all invoices
        GetInvoicesQueryResponse queryResponse = await messageManager.SendCommandAsync(new GetInvoicesQuery());
        queryResponse.Invoices.Should().HaveCount(0);
    }
    
    [TestCase("11")]
    [TestCase("XX")]
    [TestCase("000")]
    [TestCase("")]
    [TestCase("0")]
    public async Task Providing_invalid_invoice_number_when_creating_the_invoice_should_thrown_DomainException(string invalidInvoiceNumber)
    {
        // Arrange
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        IMessageManager messageManager = scopedServices.GetRequiredService<IMessageManager>();

        // Act
        DomainException ex = Assert.ThrowsAsync<DomainException>(async () =>
        {
            _ = await messageManager.SendCommandAsync(CreateFakeCreateInvoiceCommand(invalidInvoiceNumber));
        });

        ex.DomainErrors.Should().HaveCount(1);
        ex.DomainErrors.First().ErrorCode.Should().BeEquivalentTo("LengthValidator");
        ex.DomainErrors.First().PropertyName.Should().BeEquivalentTo("Invoice.InvoiceNumber");

        GetInvoicesQueryResponse result = await messageManager.SendCommandAsync(new GetInvoicesQuery());

        // Repo should be empty after throwing the exception
        result.Invoices.Should().HaveCount(0);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task Get_invoice_handler_should_be_called_the_same_number_of_times_as_get_invoice_query(int count)
    {
        // Replace the registered IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse> with a fake handler.
        // Use a counter to track the number of times the GetInvoiceQueryHandler is called.
        // Assert that the counter value matches the number of GetInvoiceQuery calls.
        
        // Arrange
        ServiceCollection services = TestHelper.PrepareServiceCollection(TestHelper.GetDefaultConfiguration());
        
        services.AddSingleton<Counter>();

        // Replace the registered event class
        ServiceDescriptor serviceDescriptor = services.First(d =>
            d.ServiceType == typeof(IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>));
        services.Remove(serviceDescriptor);
        services.AddScoped<IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>, FakeGetInvoiceHandler>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        var counter = scopedServices.GetRequiredService<Counter>();

        // Act
        for (var i = 0; i < count; i++)
        {
            await messageManager.SendCommandAsync(new GetInvoiceQuery(Guid.NewGuid().ToString()));
        }

        // Assert
        // The GetInvoiceQueryHandler should be called the same number of times as the GetInvoiceQuery
        counter.Get().Should().Be(count);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task Get_invoices_handler_should_be_called_the_same_number_of_times_as_get_invoices_query(int count)
    {
        // Replace the registered IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse> with a fake handler.
        // Use a counter to track the number of times the GetInvoicesQueryHandler is called.
        // Assert that the counter value matches the number of GetInvoicesQuery calls.
        
        // Arrange
        ServiceCollection services = TestHelper.PrepareServiceCollection(TestHelper.GetDefaultConfiguration());

        services.AddSingleton<Counter>();

        // Replace the registered event class
        ServiceDescriptor serviceDescriptor = services.First(d =>
            d.ServiceType == typeof(IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>));
        services.Remove(serviceDescriptor);
        services.AddScoped<IRequestHandler<GetInvoicesQuery, GetInvoicesQueryResponse>, FakeGetInvoicesHandler>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        var counter = scopedServices.GetRequiredService<Counter>();

        // Act
        for (var i = 0; i < count; i++)
        {
            await messageManager.SendCommandAsync(new GetInvoicesQuery());
        }

        // Assert
        // The GetInvoicesQueryHandler should be called the same number of times as the GetInvoicesQuery
        counter.Get().Should().Be(count);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task Create_invoice_handler_should_be_called_the_same_number_of_times_as_create_invoice_command(int count)
    {
        // Arrange
        ServiceCollection services = TestHelper.PrepareServiceCollection(TestHelper.GetDefaultConfiguration());

        services.AddSingleton<Counter>();

        // Replace the registered event class
        ServiceDescriptor serviceDescriptor = services.First(d =>
            d.ServiceType == typeof(IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>));
        services.Remove(serviceDescriptor);
        services.AddScoped<IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>, FakeCreateInvoiceHandler>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        var counter = scopedServices.GetRequiredService<Counter>();

        // Act
        for (var i = 0; i < count; i++)
        {
            await messageManager.SendCommandAsync(CreateFakeCreateInvoiceCommand());
        }

        // Assert
        // The CreateInvoiceCommandHandler should be called the same number of times as the CreateInvoiceCommand
        counter.Get().Should().Be(count);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task Remove_invoice_handler_should_be_called_the_same_number_of_times_as_remove_invoice_command(int count)
    {
        // Arrange
        ServiceCollection services = TestHelper.PrepareServiceCollection(TestHelper.GetDefaultConfiguration());

        services.AddSingleton<Counter>();

        // Replace the registered event class
        ServiceDescriptor serviceDescriptor = services.First(d =>
            d.ServiceType == typeof(IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>));
        services.Remove(serviceDescriptor);
        services.AddScoped<IRequestHandler<RemoveInvoiceCommand, RemoveInvoiceCommandResponse>, FakeRemoveInvoiceHandler>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        var counter = scopedServices.GetRequiredService<Counter>();

        // Act
        for (var i = 0; i < count; i++)
        {
            await messageManager.SendCommandAsync(new RemoveInvoiceCommand(Guid.NewGuid().ToString()));
        }

        // Assert
        // The RemoveInvoiceCommandHandler should be called the same number of times as the RemoveInvoiceCommand
        counter.Get().Should().Be(count);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public async Task All_created_invoices_should_be_able_to_get_by_passing_their_id2(int count)
    {
        var serviceProvider = TestHelper.SetUpServiceProviderWithDefaultInMemoryDatabase();
        
        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        await TestHelper.SetUpDatabase(scopedServices);

        var messageManager = scopedServices.GetRequiredService<IMessageManager>();

        // Arrange

        var createInvoiceResponses = new List<CreateInvoiceCommandResponse>();

        // Act
        for (var i = 0; i < count; i++)
        {
            CreateInvoiceCommandResponse createInvoiceResponse = await messageManager.SendCommandAsync(
                CreateFakeCreateInvoiceCommand());
            createInvoiceResponses.Add(createInvoiceResponse);
        }

        // Assert
        foreach (var createdInvoice in createInvoiceResponses)
        {
            GetInvoiceQueryResponse queryResponse =
                await messageManager.SendCommandAsync(new GetInvoiceQuery(createdInvoice.Invoice?.Id!));
            queryResponse.Invoice.Should().NotBeNull();
            queryResponse.Invoice?.Id.Should().BeEquivalentTo(createdInvoice.Invoice?.Id);
            queryResponse.Invoice?.InvoiceCreationEmailSent.Should().BeTrue();
        }
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