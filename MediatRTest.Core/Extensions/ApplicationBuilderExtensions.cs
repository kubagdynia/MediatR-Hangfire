using Hangfire;
using MediatRTest.Core.Configurations;
using MediatRTest.Core.Middlewares;

namespace MediatRTest.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCore(this IApplicationBuilder app, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(app);
        
        return app.UseCustomHangfire(config);
    }
    
    public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }
    
    private static IApplicationBuilder UseCustomHangfire(this IApplicationBuilder app, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(app);
        
        var hangfireConfiguration = config.GetSection(HangfireConfiguration.SectionName).Get<HangfireConfiguration>();

        if (hangfireConfiguration is null || !hangfireConfiguration.Enabled || !hangfireConfiguration.UseDashboard)
        {
            return app;
        }
        
        return app.UseHangfireDashboard("/hangfire");
    }
}