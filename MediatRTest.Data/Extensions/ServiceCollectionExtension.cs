using MediatRTest.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Data.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDbData(this IServiceCollection services, IConfiguration config)
    {
        var databaseOptions = config.GetSection("DatabaseOptions").Get<DatabaseOptions>() ??
                              throw new Exception("DatabaseOptions not found in configuration");
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(databaseOptions.ConnectionString, optBuilder =>
            {
                optBuilder.CommandTimeout(databaseOptions.CommandTimeout);
            })
            .EnableDetailedErrors(databaseOptions.EnableDetailedErrors)
            .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}