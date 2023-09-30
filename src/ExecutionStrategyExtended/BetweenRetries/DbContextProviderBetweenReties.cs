using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.BetweenRetries;

public static class BetweenRetriesStrategies
{
    public static BetweenRetiesStrategyConfiguration CreateNewDbContextStrategy(bool disposePreviousContext = false)
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static BetweenRetiesStrategyConfiguration ClearChangeTrackerStrategy()
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.ClearChangeTracker
        };
    }
    
    public static BetweenRetiesStrategyConfiguration UseSameDbContextStrategy()
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.UseSame
        };
    }
}

public class BetweenRetiesStrategyConfiguration
{
    public DbContextRetryPolicy BetweenRetiesPolicy { get; set; }
    public bool DisposePreviousContext { get; set; }
}

internal interface IBetweenRetiesStrategy<TDbContext> where TDbContext : DbContext
{
    Task<TDbContext> ProvideDbContextForRetry();
}

internal class CreateNewDbContextStrategy<TDbContext> : IBetweenRetiesStrategy<TDbContext> where TDbContext : DbContext
{
    private readonly bool _disposePreviousContext;
    private readonly IDbContextFactory<TDbContext> _factory;
    private TDbContext? _previousContext;

    public CreateNewDbContextStrategy(bool disposePreviousContext, IDbContextFactory<TDbContext> factory)
    {
        _disposePreviousContext = disposePreviousContext;
        _factory = factory;
    }
    
    public async Task<TDbContext> ProvideDbContextForRetry()
    {
        await DisposePreviousContext();

        var context = await _factory.CreateDbContextAsync();
        _previousContext = context;

        return context;
    }

    private async Task DisposePreviousContext()
    {
        if (!_disposePreviousContext)
        {
            _previousContext = null;
            return;
        }

        if (_previousContext is null)
        {
            return;
        }

        await _previousContext.DisposeAsync();
        _previousContext = null;
    }
}

internal class ClearChangeTrackerStrategy<TDbContext> : IBetweenRetiesStrategy<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ClearChangeTrackerStrategy(TDbContext context)
    {
        _context = context;
    }
    
    public Task<TDbContext> ProvideDbContextForRetry()
    {
        _context.ChangeTracker.Clear();
        return Task.FromResult(_context);
    }
}

internal class UseSameDbContextStrategy<TDbContext> : IBetweenRetiesStrategy<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UseSameDbContextStrategy(TDbContext context)
    {
        _context = context;
    }
    
    public Task<TDbContext> ProvideDbContextForRetry()
    {
        return Task.FromResult(_context);
    }
}