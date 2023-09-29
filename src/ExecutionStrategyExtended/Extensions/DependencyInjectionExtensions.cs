using ExecutionStrategyExtended.Configuration;
using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.ExecutionStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services, 
        ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, _) => { }, lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IExecutionStrategyPublicConfiguration> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options), lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IExecutionStrategyPublicConfiguration> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddOptions();

        services
            .AddTransient<IConfigureOptions<ExecutionStrategyExtendedConfiguration>,
                ExecutionStrategyExtendedConfigure>(
                provider => new ExecutionStrategyExtendedConfigure(provider, action));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyInternalConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyPublicConfiguration), Factory, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(MainFactory), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(TrueExecutionStrategyFactory<TDbContext>),
            provider => provider.GetRequiredService<MainFactory>()
                .CreateTrueExecutionStrategyFactory<TDbContext>(),
            lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            provider => provider.GetRequiredService<MainFactory>()
                .CreateExecutionStrategyExtended<TDbContext>(),
            lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedConfiguration Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedConfiguration>>();
        return factory.Create(Options.DefaultName);
    }
}