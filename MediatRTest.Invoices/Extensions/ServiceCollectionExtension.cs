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
                new Invoice(id: Guid.NewGuid().ToString(), number: "J/1/2024", creationDate: new DateTime(2024, 10, 1)),
                new Invoice(id: Guid.NewGuid().ToString(), number: "J/2/2024", creationDate: new DateTime(2024, 10, 25)),
                new Invoice(id: Guid.NewGuid().ToString(), number: "J/3/2034", creationDate: new DateTime(2024, 11, 1))
            ]
        ));

        return services;
    }
}