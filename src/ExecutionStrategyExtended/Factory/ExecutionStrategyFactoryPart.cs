using ExecutionStrategyExtended.BetweenRetryDbContext;
using ExecutionStrategyExtended.Configuration.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.Factory;

internal class ExecutionStrategyFactoryPart
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;
    private readonly IServiceProvider _provider;

    public ExecutionStrategyFactoryPart(IExecutionStrategyInternalConfiguration configuration, IServiceProvider provider)
    {
        _configuration = configuration;
        _provider = provider;
    }
    
    public IBetweenRetryDbContextBehavior<TDbContext> CreateBetweenRetriesStrategy<TDbContext>(TDbContext mainContext) where TDbContext : DbContext
    {
        return _configuration.BetweenRetryDbContextBehaviorConfiguration.BetweenRetryDbContextBehaviorType switch
        {
            BetweenRetryDbContextBehaviorType.CreateNew => new CreateNewDbContextBehavior<TDbContext>(
                _configuration.BetweenRetryDbContextBehaviorConfiguration.DisposePreviousContext,
                _provider.GetRequiredService<IDbContextFactory<TDbContext>>(), mainContext),
            BetweenRetryDbContextBehaviorType.ClearChangeTracker => new ClearChangeTrackerBehavior<TDbContext>(mainContext),
            BetweenRetryDbContextBehaviorType.UseSame => new UseSameDbContextBehavior<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}