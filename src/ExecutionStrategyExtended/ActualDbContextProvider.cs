using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

internal class ActualDbContextProvider<TDbContext> : IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private TDbContext? _dbContext;

    public TDbContext DbContext
    {
        get => _dbContext!;
        set => _dbContext = value;
    }
}