using MediatRTest.Data.Options;
using Microsoft.Data.Sqlite;
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
            // Use an in-memory database for testing
            if (databaseOptions.InMemoryDatabase)
            {
                var connectionString = new SqliteConnection("Filename=:memory:");
                connectionString.Open();
                options.UseSqlite(connectionString, optBuilder =>
                {
                    optBuilder.CommandTimeout(databaseOptions.CommandTimeout);
                });
            }
            else
            {
                // Use a SQLite database for production
                options.UseSqlite(databaseOptions.ConnectionString, optBuilder =>
                {
                    optBuilder.CommandTimeout(databaseOptions.CommandTimeout);
                });
            }

            options
                .EnableDetailedErrors(databaseOptions.EnableDetailedErrors)
                .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}