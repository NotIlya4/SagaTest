using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.BetweenRetries;

internal class ClearChangeTrackerBehavior<TDbContext> : IBetweenRetryDbContextBehavior<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ClearChangeTrackerBehavior(TDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public Task<TDbContext> ProvideDbContextForRetry(int retry)
    {
        _context.ChangeTracker.Clear();
        return Task.FromResult(_context);
    }
}