using System.Reflection;
using MediatRTest.Core.Extensions;
using MediatRTest.Invoices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInvoices(this IServiceCollection services, bool registerValidators = true)
    {
        services.AddCore(assembly: Assembly.GetExecutingAssembly(), registerValidators: registerValidators);

        services.AddSingleton<IInvoiceRepository>(c => new InvoiceMemoryRepository());

        return services;
    }
}