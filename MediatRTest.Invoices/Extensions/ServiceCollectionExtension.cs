using System.Reflection;
using MediatRTest.Core.Extensions;
using MediatRTest.Invoices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInvoices(this IServiceCollection services, bool registerValidators = true)
    {
        // Register the core services
        services.AddCore(assembly: Assembly.GetExecutingAssembly(), registerValidators: registerValidators);

        // Register the invoice services
        services.AddSingleton<IInvoiceRepository>(c => new InvoiceMemoryRepository());

        return services;
    }
}