using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace MediatRTest.Api.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing ApplicationDbContext registration and replace it with a SQLite in-memory database.
            // Commented out because by default we use an in-memory database, which is set in the configuration
            
            // var dbContextDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbContextOptions<ApplicationDbContext>));
            //
            // services.Remove(dbContextDescriptor);
            //
            // var dbConnectionDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbConnection));
            //
            // services.Remove(dbConnectionDescriptor);
            //
            // // Create open SqliteConnection so EF won't automatically close it.
            // services.AddSingleton<DbConnection>(container =>
            // {
            //     var connection = new SqliteConnection("DataSource=:memory:");
            //     connection.Open();
            //
            //     return connection;
            // });
            //
            // services.AddDbContext<ApplicationDbContext>((container, options) =>
            // {
            //     var connection = container.GetRequiredService<DbConnection>();
            //     options.UseSqlite(connection);
            // });
        });

        var testConfiguration = new Dictionary<string, string?>
        {
            { "Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information" },
            { "DatabaseOptions:ConnectionString", "Data Source=sqlite.db" },
            { "DatabaseOptions:InMemoryDatabase", "true" },
            { "DatabaseOptions:CommandTimeout", "10" },
            { "DatabaseOptions:EnableSensitiveDataLogging", "false" },
            { "DatabaseOptions:EnableDetailedErrors", "false" }
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(testConfiguration).Build();
        builder.UseConfiguration(configuration);

        builder.UseEnvironment("Development");
    }
}