using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Models;
using ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddIdempotentTransactionModels(this ModelBuilder builder)
    {
        builder.Entity<IdempotencyToken>().Property(x => x.Id).HasMaxLength(255);
    }

    public static IServiceCollection AddIdempotentTransactionServices<TDbContext>(this IServiceCollection services, 
        ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddIdempotentTransactionServices<TDbContext>((provider, options) => { }, lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddIdempotentTransactionServices<TDbContext>(this IServiceCollection services,
        Action<IdempotentTransactionOptions> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddIdempotentTransactionServices<TDbContext>((provider, options) => action(options), lifetimeOverride);

        return services;
    }

    public static IServiceCollection AddIdempotentTransactionServices<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IdempotentTransactionOptions> action, ServiceLifetime lifetimeOverride = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services.AddOptions();

        services
            .AddTransient<IConfigureOptions<IdempotentTransactionOptions>, IdempotentTransactionOptionsConfigure>(
                provider => new IdempotentTransactionOptionsConfigure(provider, action));

        services.PostConfigure<IdempotentTransactionOptions>(options => options.Validate());

        services.Add(new ServiceDescriptor(typeof(IdempotentTransactionOptions), Factory, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IdempotencyFactory), provider =>
        {
            var options = provider.GetRequiredService<IdempotentTransactionOptions>();
            return new IdempotencyFactory(options.SystemClock.Value!, options.ResponseSerializer.Value!);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(DbContextFactoryBetweenReties<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<IdempotentTransactionOptions>();
            var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
            return new DbContextFactoryBetweenReties<TDbContext>(
                options.BetweenRetriesBehavior.Value!.DisposeContextOnCreateNew, dbContextFactory);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(TrueExecutionStrategyFactory<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<IdempotentTransactionOptions>();
            var factory = provider.GetRequiredService<DbContextFactoryBetweenReties<TDbContext>>();
            return new TrueExecutionStrategyFactory<TDbContext>(options.BetweenRetriesBehavior.Value!.RetryPolicy,
                factory);
        }, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>), provider =>
        {
            var options = provider.GetRequiredService<IdempotentTransactionOptions>();
            var idempotencyFactory = provider.GetRequiredService<IdempotencyFactory>();
            var strategyFactory = provider.GetRequiredService<TrueExecutionStrategyFactory<TDbContext>>();

            return new ExecutionStrategyExtended<TDbContext>(idempotencyFactory,
                options.IdempotencyViolationDetector.Value!, options.ResponseSerializer.Value!, strategyFactory);
        }, lifetimeOverride));

        return services;
    }

    private static IdempotentTransactionOptions Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<IdempotentTransactionOptions>>();
        return factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
    }
}