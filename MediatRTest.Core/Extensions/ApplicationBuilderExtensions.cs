using MediatRTest.Core.Middlewares;

namespace MediatRTest.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}