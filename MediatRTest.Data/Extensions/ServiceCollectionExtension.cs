using System.Data.Common;
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
        
        if (databaseOptions.InMemoryDatabase)
        {
            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });
        }
        
        services.AddDbContext<DataContext>((container, options) =>
        {
            // Use an in-memory database for testing
            if (databaseOptions.InMemoryDatabase)
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection, optBuilder =>
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
                .EnableDetailedErrors(databaseOptions.EnableDetailedErrors) // Enable or Disable detailed errors
                .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging) // Enable or Disable sensitive data logging
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Disable tracking by default
        });

        return services;
    }
}