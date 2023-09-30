using ExecutionStrategyExtended.BetweenRetries;
using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.Configuration;

internal class MainFactory
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;
    private readonly IServiceProvider _provider;

    public MainFactory(IExecutionStrategyInternalConfiguration configuration, IServiceProvider provider)
    {
        _configuration = configuration;
        _provider = provider;
    }

    public IdempotencyTokenManager CreateIdempotencyFactory()
    {
        return new IdempotencyTokenManager(_configuration.SystemClock, _configuration.ResponseSerializer);
    }

    public DbContextFactoryBetweenReties<TDbContext> CreateDbContextFactoryBetweenReties<TDbContext>()
        where TDbContext : DbContext
    {
        return new DbContextFactoryBetweenReties<TDbContext>(
            _configuration.BetweenRetriesBehaviorConfiguration.DisposeContextOnCreateNew,
            GetFactory<TDbContext>());
    }

    public TrueExecutionStrategyFactory<TDbContext> CreateTrueExecutionStrategyFactory<TDbContext>()
        where TDbContext : DbContext
    {
        return new TrueExecutionStrategyFactory<TDbContext>(
            _configuration.BetweenRetriesBehaviorConfiguration.RetryPolicy,
            CreateDbContextFactoryBetweenReties<TDbContext>());
    }

    public ExecutionStrategyExtended<TDbContext> CreateExecutionStrategyExtended<TDbContext>()
        where TDbContext : DbContext
    {
        return new ExecutionStrategyExtended<TDbContext>(
            CreateIdempotencyFactory(), _configuration.IdempotenceViolationDetector, _configuration.ResponseSerializer,
            CreateTrueExecutionStrategyFactory<TDbContext>());
    }

    private IDbContextFactory<TDbContext> GetFactory<TDbContext>() where TDbContext : DbContext
    {
        return _provider.GetRequiredService<IDbContextFactory<TDbContext>>();
    }
}