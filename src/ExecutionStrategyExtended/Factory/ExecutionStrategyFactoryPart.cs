using ExecutionStrategyExtended.Configuration.Interfaces;
using ExecutionStrategyExtended.DbContextRetrier;
using ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;
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
    
    public IDbContextRetrier<TDbContext> CreateDbContextRetrier<TDbContext>(TDbContext mainContext) where TDbContext : DbContext
    {
        return _configuration.DbContextRetrierConfiguration.DbContextRetrierType switch
        {
            DbContextRetrierType.CreateNew => new CreateNewDbContextRetrier<TDbContext>(
                _configuration.DbContextRetrierConfiguration.DisposePreviousContext,
                _provider.GetRequiredService<IDbContextFactory<TDbContext>>(), mainContext),
            DbContextRetrierType.ClearChangeTracker => new ClearChangeTrackerRetrier<TDbContext>(mainContext),
            DbContextRetrierType.UseSame => new UseSameDbContextRetrier<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}