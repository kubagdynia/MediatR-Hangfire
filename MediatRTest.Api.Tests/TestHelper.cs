using System.Text.Json;
using MediatRTest.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRTest.Api.Tests;

internal static class TestHelper
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    internal static List<Error> DeserializeErrors(object? errors)
        => JsonSerializer.Deserialize<List<Error>>(errors!.ToString()!, TestHelper.JsonSerializerOptions)!;
    
    internal static async void CreateAndSeedDatabase(WebApplicationFactory<Program> appFactory)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        DataContext dataContext = scopedServices.GetRequiredService<DataContext>();
        await dataContext.Database.EnsureCreatedAsync();
    }
    
    internal sealed record Error(string? Code, string? Title, string? Details, string? UserMessage);
}