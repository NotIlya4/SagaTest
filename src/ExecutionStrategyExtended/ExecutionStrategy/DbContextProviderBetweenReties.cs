using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public class DbContextProviderBetweenReties<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _mainContext;
    private readonly DbContextRetryPolicy _policy;
    private readonly DbContextFactoryBetweenReties<TDbContext> _factoryBetweenReties;
    public TDbContext MainContext { get => _mainContext; }

    public DbContextProviderBetweenReties(TDbContext mainContext, DbContextRetryPolicy policy, DbContextFactoryBetweenReties<TDbContext> factoryBetweenReties)
    {
        _mainContext = mainContext;
        _policy = policy;
        _factoryBetweenReties = factoryBetweenReties;
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
        return await _factoryBetweenReties.Create();
    }
}