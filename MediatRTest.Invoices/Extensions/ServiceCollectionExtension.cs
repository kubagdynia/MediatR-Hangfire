using System.Reflection;
using MediatRTest.Core.Extensions;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Invoices.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInvoices(this IServiceCollection services, bool registerValidators = true)
    {
        services.AddCore(assembly: Assembly.GetExecutingAssembly(), registerValidators: true);

        services.AddSingleton<IInvoiceRepository>(c => new InvoiceMemoryRepository(
            [
                new DbInvoice(Guid.NewGuid().ToString(), "J/1/2024", 150m,  new DateTime(2024, 10, 1)),
                new DbInvoice(Guid.NewGuid().ToString(), "J/2/2024", 60.50m, new DateTime(2024, 10, 25)),
                new DbInvoice(Guid.NewGuid().ToString(), "J/3/2034", 250.0m, new DateTime(2024, 11, 1))
            ]
        ));

        return services;
    }
}