using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediatRTest.Core.Serializers;

public static class BaseJsonOptions
{
    public static JsonIgnoreCondition DefaultIgnoreCondition { get; } = JsonIgnoreCondition.WhenWritingNull;
    public static JsonNamingPolicy PropertyNamingPolicy { get; } = JsonNamingPolicy.CamelCase;

    public static JsonSerializerOptions GetJsonSerializerOptions { get; } = new()
    {
        DefaultIgnoreCondition = DefaultIgnoreCondition,
        PropertyNamingPolicy = PropertyNamingPolicy,
    };
}