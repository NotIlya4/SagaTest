using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public interface IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; set; }
}