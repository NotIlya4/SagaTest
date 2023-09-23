using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public class BetweenRetiesDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _mainContext;
    private readonly DbContextRetryPolicy _policy;
    private readonly DbContextFactory<TDbContext> _factory;
    public TDbContext MainContext { get => _mainContext; }

    public BetweenRetiesDbContextProvider(TDbContext mainContext, DbContextRetryPolicy policy, DbContextFactory<TDbContext> factory)
    {
        _mainContext = mainContext;
        _policy = policy;
        _factory = factory;
    }

    public async Task<TDbContext> ProvideDbContext()
    {
        return _policy switch
        {
            DbContextRetryPolicy.UseSame => ProvideForSame(),
            DbContextRetryPolicy.ClearChangeTracker => ProvideForClear(),
            DbContextRetryPolicy.CreateNew => await ProvideForNew(),
            _ => throw new NotImplementedException()
        };
    }

    private TDbContext ProvideForSame()
    {
        return _mainContext;
    }

    private TDbContext ProvideForClear()
    {
        _mainContext.ChangeTracker.Clear();
        return _mainContext;
    }

    private async Task<TDbContext> ProvideForNew()
    {
        return await _factory.Create();
    }
}