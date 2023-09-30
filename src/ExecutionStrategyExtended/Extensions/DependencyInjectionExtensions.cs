using ExecutionStrategyExtended.Configuration;
using ExecutionStrategyExtended.Configuration.Interfaces;
using ExecutionStrategyExtended.Factory;
using ExecutionStrategyExtended.StrategyExtended;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, _) => { });

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IExecutionStrategyPublicConfiguration> action) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options));

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IExecutionStrategyPublicConfiguration> action) where TDbContext : DbContext
    {
        var lifetimeOverride = ServiceLifetime.Scoped;
        
        services.AddOptions();

        services
            .AddTransient<IConfigureOptions<ExecutionStrategyExtendedConfiguration>,
                ExecutionStrategyExtendedConfigure>(
                provider => new ExecutionStrategyExtendedConfigure(provider, action));

        services.Add(new ServiceDescriptor(typeof(ActualDbContextProvider<TDbContext>), typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IActualDbContextProvider<TDbContext>), typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        
        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyInternalConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyPublicConfiguration), Factory, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(MainFactory<TDbContext>), typeof(MainFactory<TDbContext>), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            typeof(ExecutionStrategyExtended<TDbContext>),
            lifetimeOverride));
        
        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtended<TDbContext>), typeof(ExecutionStrategyExtended<TDbContext>), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedConfiguration Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedConfiguration>>();
        return factory.Create(Options.DefaultName);
    }
}