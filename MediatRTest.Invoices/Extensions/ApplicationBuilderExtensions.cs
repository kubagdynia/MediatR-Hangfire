using MediatRTest.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace MediatRTest.Invoices.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseInvoices(this IApplicationBuilder app, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UseCore(config);
    }
}