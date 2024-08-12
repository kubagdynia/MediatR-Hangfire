using System.Reflection;
using MediatRTest.Core.Extensions;
using MediatRTest.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInvoices(this IServiceCollection services, IConfiguration config, bool registerValidators = true)
    {
        // Register the core services
        services.AddCore(assembly: Assembly.GetExecutingAssembly(), registerValidators: registerValidators);

        // Register the data services (database)
        services.AddDbData(config);

        return services;
    }
}