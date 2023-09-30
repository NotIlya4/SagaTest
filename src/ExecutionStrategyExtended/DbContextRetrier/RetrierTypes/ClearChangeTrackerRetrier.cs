using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;

internal class ClearChangeTrackerRetrier<TDbContext> : IDbContextRetrier<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ClearChangeTrackerRetrier(TDbContext context)
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