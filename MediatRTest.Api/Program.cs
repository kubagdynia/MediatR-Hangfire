using System.Reflection;
using System.Text.Json.Serialization;
using Asp.Versioning;
using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Exceptions;
using MediatRTest.Data;
using MediatRTest.Invoices.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container for configuration and dependency injection
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, false)
    .AddCommandLine(args)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer() // Add the endpoint API explorer
    .AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
    .AddExceptionHandler<GlobalExceptionHandler>() // Add the global exception handler
    .AddProblemDetails() // Add the problem details middleware
    .AddEndpoints(Assembly.GetExecutingAssembly()) // Add the endpoints
    .AddInvoices(builder.Configuration, registerValidators: true) // Add the invoices services
    .ConfigureHttpJsonOptions(opt =>
    {
        opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

// defining Serilog configs
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// configure logging
builder.Services.AddLogging(b =>
{ 
    b.AddSerilog();
});

// Build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwaggerUI(); // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    
    // Create the database if it doesn't exist and apply any pending migration.
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS

//app.UseErrorHandlerMiddleware(); // Use the error handler middleware
app.UseExceptionHandler(); // Use the global exception handler

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

var versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

app.Run();

Log.CloseAndFlush();

public partial class Program;