using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.ExecutionStrategy;

internal class TrueExecutionStrategy<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextProviderBetweenReties<TDbContext> _contextProvider;

    public TrueExecutionStrategy(DbContextProviderBetweenReties<TDbContext> contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<TDbContext, Task<TResult>> operation)
    {
        var strategy = _contextProvider.MainContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await _contextProvider.ProvideDbContext();
                return await operation(context);
            });
    }
}