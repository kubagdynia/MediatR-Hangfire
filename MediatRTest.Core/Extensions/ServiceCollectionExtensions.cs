using System.Reflection;
using FluentValidation;
using MediatR.Pipeline;
using MediatRTest.Core.Behaviors;
using MediatRTest.Core.Logs;
using MediatRTest.Core.Messages;

namespace MediatRTest.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => AddCore(services, []);
    
    public static IServiceCollection AddCore(this IServiceCollection services, Assembly assembly, bool registerValidators = false)
        => AddCore(services, [assembly], registerValidators);
    
    public static IServiceCollection AddCore(this IServiceCollection services, Assembly[]? assemblies,
        bool registerValidators = false)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetExecutingAssembly()];
        }
        
        if (registerValidators)
        {
            services.AddValidatorsFromAssemblies(assemblies);
        }

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(RequestPostProcessorBehavior<,>));
        });
        
        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(LoggingPostProcessor<,>));
        
        services.AddScoped<IMessageManager, MessageManager>();

        return services;
    }
}