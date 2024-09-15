using System.Reflection;
using FluentValidation;
using Hangfire;
using MediatR.NotificationPublishers;
using MediatR.Pipeline;
using MediatRTest.Core.Behaviors;
using MediatRTest.Core.Configurations;
using MediatRTest.Core.Logging;
using MediatRTest.Core.Messages;

namespace MediatRTest.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
        => AddCore(services, assemblies: [], config);
    
    public static IServiceCollection AddCore(this IServiceCollection services, Assembly assembly, IConfiguration config, bool registerValidators = false)
        => AddCore(services, assemblies: [assembly], config, registerValidators);
    
    public static IServiceCollection AddCore(this IServiceCollection services, Assembly[]? assemblies, IConfiguration config,
        bool registerValidators = false)
    {
        // If assemblies are not specified, use the executing assembly.
        if (assemblies == null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetExecutingAssembly()];
        }
        
        // Register validators if specified
        if (registerValidators)
        {
            // Disable the language manager to prevent the default language manager from being registered.
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            // Register validators from the specified assemblies.
            services.AddValidatorsFromAssemblies(assemblies);
        }

        // Register MediatR and behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestPostProcessorBehavior<,>));
            // TaskWhenAllPublisher means that notifications will be published in parallel, and this mechanism
            // will wait for all tasks to complete before proceeding with further operations.
            // This approach ensures that all notification handlers are executed in parallel, which can reduce
            // the overall execution time, especially when handlers perform independent asynchronous operations.
            // Note! If you want to publish notifications in sequence, you can use the NoOpPublisher.
            cfg.NotificationPublisher = new TaskWhenAllPublisher(); // Default is new NoOpPublisher();  
        });
        
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Register logging behavior for MediatR
        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(LoggingPostProcessor<,>));
        
        services.AddHangfire(config);
        
        // Register message services
        services.AddScoped<IMessageExecutor, MessageExecutor>();
        services.AddScoped<IMessageManager, MessageManager>();

        return services;
    }
    
    private static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration config)
    {
        // Get the Hangfire configuration from the appsettings.json file.
        var hangfireConfigurationSection = config.GetSection(HangfireConfiguration.SectionName);
        var hangfireConfiguration = hangfireConfigurationSection.Get<HangfireConfiguration>();
        
        // If Hangfire is not enabled, return the services without adding Hangfire.
        if (hangfireConfiguration is null || !hangfireConfiguration.Enabled)
        {
            return services;
        }

        // Register the Hangfire configuration.
        services.Configure<HangfireConfiguration>(hangfireConfigurationSection);
        
        // Add Hangfire services.
        services.AddHangfire(cfg =>
        {
            // Use the in-memory storage if the configuration specifies it.
            // Otherwise, use the SQL Server storage.
            if (hangfireConfiguration.UseInMemoryStorage)
            {
                cfg.UseInMemoryStorage();
            }
            else
            {
                cfg.UseSqlServerStorage(hangfireConfiguration.ConnectionString);
            }
        });
        
        services.AddHangfireServer(opt =>
        {
            // Set the interval between the server's heartbeat.
            opt.HeartbeatInterval = TimeSpan.FromMinutes(1);
            
            // Set the maximum number of worker threads.
            if (hangfireConfiguration.MaxDefaultWorkerCount > 0)
            {
                // Set the number of worker threads to the minimum of the number of processors multiplied by 5 and the maximum number of worker threads.
                // This is done to prevent the server from using too many resources.
                opt.WorkerCount = Math.Min(Environment.ProcessorCount * 5, hangfireConfiguration.MaxDefaultWorkerCount);
            }

            // Set the queues that the server will process.
            // If the queues are not specified, the server will process the "default" queue.
            opt.Queues = hangfireConfiguration.Queues is { Length: > 0 } ? hangfireConfiguration.Queues : ["default"];
        });

        return services;
    }
}