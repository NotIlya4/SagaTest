using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.ExecutionStrategy;

internal class DbContextFactoryBetweenReties<TDbContext> where TDbContext : DbContext
{
    private readonly bool _disposePreviousContext;
    private readonly IDbContextFactory<TDbContext> _factory;
    private DbContext? _previousContext;

    public DbContextFactoryBetweenReties(bool disposePreviousContext, IDbContextFactory<TDbContext> factory)
    {
        _disposePreviousContext = disposePreviousContext;
        _factory = factory;
    }

    public async Task<TDbContext> Create()
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