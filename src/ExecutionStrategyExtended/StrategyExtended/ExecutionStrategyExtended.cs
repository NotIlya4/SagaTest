using System.Data;
using ExecutionStrategyExtended.Factory;
using ExecutionStrategyExtended.IdempotenceToken;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.StrategyExtended;

internal class ExecutionStrategyExtended<TDbContext> : IExecutionStrategyExtended<TDbContext>
    where TDbContext : DbContext
{
    private readonly MainFactory _factory;
    private readonly Lazy<IdempotencyTokenManager> _tokenManager;

    public ExecutionStrategyExtended(MainFactory factory)
    {
        _factory = factory;
        _tokenManager = new Lazy<IdempotencyTokenManager>(() => _factory.IdempotencyToken.CreateManager());
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        var retrier = _factory.ExecutionStrategy.CreateDbContextRetrier(mainContext);
        var strategy = retrier.CreateExecutionStrategy();
        int retryNumber = 1;

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await retrier.ProvideDbContextForRetry(retryNumber);
                retryNumber += 1;
                return await action(context);
            });
    }

    public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel)
    {
        return await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await action(context);

            await transaction.CommitAsync();

            return response;
        }, mainContext);
    }

    public async Task<TResponse> ExecuteInIdempotentTransactionAsync<TResponse>(
        Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, string idempotencyToken, IsolationLevel isolationLevel)
    {
        
        var token = _tokenManager.Value.CreateIdempotencyToken(idempotencyToken);

        return await ExecuteInTransactionAsync(async context =>
            {
                var service = _factory.IdempotencyToken.CreateService(context);
                return await service.HandleAction(async () => await action(context), token);
            },
            mainContext, isolationLevel);
    }
}