using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public class TrueExecutionStrategyFactory<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextRetryPolicy _policy;
    private readonly DbContextFactory<TDbContext> _factory;

    public TrueExecutionStrategyFactory(DbContextRetryPolicy policy, DbContextFactory<TDbContext> factory)
    {
        _policy = policy;
        _factory = factory;
    }
    
    public TrueExecutionStrategy<TDbContext> Create(TDbContext mainContext)
    {
        var contextProvider = new BetweenRetiesDbContextProvider<TDbContext>(mainContext, _policy, _factory);
        return new TrueExecutionStrategy<TDbContext>(contextProvider);
    }
}