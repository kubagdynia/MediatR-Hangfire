using MediatRTest.Core.Exceptions;
using MediatRTest.Data;
using MediatRTest.Invoices.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Tests;

public static class TestHelper
{
    internal static ServiceProvider PrepareServiceProvider(Dictionary<string, string?>? config = null)
    {
        ServiceCollection services = PrepareServiceCollection(config);
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
    
    internal static ServiceCollection PrepareServiceCollection(Dictionary<string, string?>? config = null)
    {
        ServiceCollection services = [];

        Dictionary<string, string?> testConfiguration = config ?? new Dictionary<string, string?>();
        
        IConfigurationRoot configuration = 
            new ConfigurationBuilder().AddInMemoryCollection(testConfiguration).Build();
        
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddInvoices(configuration, registerValidators: true)
            .AddLogging();
        

        return services;
    }
    
    internal static Dictionary<string, string?> GetDefaultConfiguration()
    {
        return new Dictionary<string, string?>
        {
            {"Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information"},
            {"DatabaseOptions:ConnectionString", "Data Source=sqlite.db"},
            {"DatabaseOptions:CommandTimeout", "10"},
            {"DatabaseOptions:EnableSensitiveDataLogging", "true"},
            {"DatabaseOptions:EnableDetailedErrors", "true"}
        };
    }
    
    internal static async Task SetUpDatabase(IServiceProvider scopedServices)
    {
        DataContext dataContext = scopedServices.GetRequiredService<DataContext>();
        await dataContext.Database.EnsureCreatedAsync();
    }
    
    internal static ServiceProvider SetUpServiceProviderWithDefaultInMemoryDatabase()
    {
        // Set up the configuration for the test
        // This configuration will use an in-memory database
        Dictionary<string, string?> testConfiguration = new Dictionary<string, string?>
        {
            {"Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information"},
            {"DatabaseOptions:ConnectionString", "Data Source=sqlite.db"},
            {"DatabaseOptions:InMemoryDatabase", "true"},
            {"DatabaseOptions:CommandTimeout", "10"},
            {"DatabaseOptions:EnableSensitiveDataLogging", "false"},
            {"DatabaseOptions:EnableDetailedErrors", "false"}
        };
        
        ServiceProvider serviceProvider = PrepareServiceProvider(testConfiguration);
        return serviceProvider;
    }
}