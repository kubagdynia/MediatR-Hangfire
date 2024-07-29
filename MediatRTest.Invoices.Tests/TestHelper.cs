using MediatRTest.Core.Exceptions;
using MediatRTest.Invoices.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Tests;

public static class TestHelper
{
    public static ServiceProvider PrepareServiceProvider()
    {
        ServiceCollection services = PrepareServiceCollection();
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
    
    public static ServiceCollection PrepareServiceCollection()
    {
        ServiceCollection services = new ServiceCollection();

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddInvoices(registerValidators: true)
            .AddLogging();
        

        return services;
    }
}