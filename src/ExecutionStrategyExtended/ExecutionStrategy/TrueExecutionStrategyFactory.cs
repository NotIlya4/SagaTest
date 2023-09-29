using ExecutionStrategyExtended.BetweenRetries;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.ExecutionStrategy;

internal class TrueExecutionStrategyFactory<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextRetryPolicy _policy;
    private readonly DbContextFactoryBetweenReties<TDbContext> _factoryBetweenReties;

    public TrueExecutionStrategyFactory(DbContextRetryPolicy policy, DbContextFactoryBetweenReties<TDbContext> factoryBetweenReties)
    {
        _policy = policy;
        _factoryBetweenReties = factoryBetweenReties;
    }
    
    public TrueExecutionStrategy<TDbContext> Create(TDbContext mainContext)
    {
        var contextProvider = new DbContextProviderBetweenReties<TDbContext>(mainContext, _policy, _factoryBetweenReties);
        return new TrueExecutionStrategy<TDbContext>(contextProvider);
    }
}