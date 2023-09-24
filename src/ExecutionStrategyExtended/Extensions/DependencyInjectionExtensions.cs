using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Models;
using ExecutionStrategyExtended.Options;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services, 
        ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((provider, options) => { }, lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<ExecutionStrategyExtendedOptions> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((provider, options) => action(options), lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, ExecutionStrategyExtendedOptions> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddOptions();

        services
            .AddTransient<IConfigureOptions<ExecutionStrategyExtendedOptions>, ExecutionStrategyExtendedOptionsConfigure>(
                provider => new ExecutionStrategyExtendedOptionsConfigure(provider, action));

        services.PostConfigure<ExecutionStrategyExtendedOptions>(options => options.Validate());

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedOptions), Factory, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IdempotencyFactory), provider =>
        {
            var options = provider.GetRequiredService<ExecutionStrategyExtendedOptions>();
            return new IdempotencyFactory(options.SystemClock.Value!, options.ResponseSerializer.Value!);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(DbContextFactoryBetweenReties<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<ExecutionStrategyExtendedOptions>();
            var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
            return new DbContextFactoryBetweenReties<TDbContext>(
                options.BetweenRetriesBehavior.Value!.DisposeContextOnCreateNew, dbContextFactory);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(TrueExecutionStrategyFactory<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<ExecutionStrategyExtendedOptions>();
            var factory = provider.GetRequiredService<DbContextFactoryBetweenReties<TDbContext>>();
            return new TrueExecutionStrategyFactory<TDbContext>(options.BetweenRetriesBehavior.Value!.RetryPolicy,
                factory);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<ExecutionStrategyExtendedOptions>();
            var idempotencyFactory = provider.GetRequiredService<IdempotencyFactory>();
            var strategyFactory = provider.GetRequiredService<TrueExecutionStrategyFactory<TDbContext>>();

            return new ExecutionStrategyExtended<TDbContext>(idempotencyFactory,
                options.IdempotencyViolationDetector.Value!, options.ResponseSerializer.Value!, strategyFactory);
        }, lifetimeOverride));
        
        services.Add(new ServiceDescriptor(typeof(DefaultIdempotencyViolationDetector), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IIdempotencyViolationDetector),
            typeof(DefaultPostgresIdempotencyViolationDetector), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedOptions Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedOptions>>();
        return factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
    }
}