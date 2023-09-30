using ExecutionStrategyExtended.BetweenRetries;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.ExecutionStrategy;

internal class TrueExecutionStrategy<TDbContext> where TDbContext : DbContext
{
    private readonly IBetweenRetiesStrategy<TDbContext> _contextProvider;

    public TrueExecutionStrategy(IBetweenRetiesStrategy<TDbContext> contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<TDbContext, Task<TResult>> operation)
    {
        var strategy = _contextProvider.CreateExecutionStrategy();
        int retryNumber = 1;

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await _contextProvider.ProvideDbContextForRetry(retryNumber);
                retryNumber += 1;
                return await operation(context);
            });
    }
}