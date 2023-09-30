using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.BetweenRetries;

internal class UseSameDbContextBehavior<TDbContext> : IBetweenRetryDbContextBehavior<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UseSameDbContextBehavior(TDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public Task<TDbContext> ProvideDbContextForRetry(int retry)
    {
        return Task.FromResult(_context);
    }
}