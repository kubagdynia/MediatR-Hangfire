using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MediatRTest.Api.Tests;

public class CustomWebApplicationFactory<TProgram>(Dictionary<string, string?> appConfig) : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            // Clear the default configuration sources
            config.Sources.Clear();
            
            // Add the configuration from the test project
            config.AddInMemoryCollection(appConfig);
        });
        
        builder.UseEnvironment("Production"); // Development, Staging, or Production
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#customize-webapplicationfactory
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
        
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(appConfig).Build();
        builder.UseConfiguration(configuration);
        builder.UseEnvironment("Production"); // Development, Staging, or Production
    }
}