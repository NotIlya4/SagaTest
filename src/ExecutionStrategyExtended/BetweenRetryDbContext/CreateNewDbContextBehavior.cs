using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.BetweenRetryDbContext;

internal class CreateNewDbContextBehavior<TDbContext> : IBetweenRetryDbContextBehavior<TDbContext> where TDbContext : DbContext
{
    private readonly bool _disposePreviousContext;
    private readonly IDbContextFactory<TDbContext> _factory;
    private readonly TDbContext _mainContext;
    private TDbContext? _previousContext;

    public CreateNewDbContextBehavior(bool disposePreviousContext, IDbContextFactory<TDbContext> factory, TDbContext mainContext)
    {
        _disposePreviousContext = disposePreviousContext;
        _factory = factory;
        _mainContext = mainContext;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _mainContext.Database.CreateExecutionStrategy();
    }

    public async Task<TDbContext> ProvideDbContextForRetry(int retry)
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