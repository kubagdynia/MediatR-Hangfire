using MediatRTest.Core.Exceptions;
using MediatRTest.Invoices.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Tests;

public static class TestHelper
{
    public static ServiceProvider PrepareServiceProvider(Dictionary<string, string?>? config = null)
    {
        ServiceCollection services = PrepareServiceCollection(config);
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
    
    public static ServiceCollection PrepareServiceCollection(Dictionary<string, string?>? config = null)
    {
        var services = new ServiceCollection();

        var testConfiguration = config ?? new Dictionary<string, string?>();
        
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(testConfiguration).Build();
        
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddInvoices(configuration, registerValidators: true)
            .AddLogging();
        

        return services;
    }
    
    public static Dictionary<string, string?> GetDefaultConfiguration()
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
}