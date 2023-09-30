using System.Data;
using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

internal class ExecutionStrategyExtended<TDbContext> : IExecutionStrategyExtended<TDbContext>
    where TDbContext : DbContext
{
    private readonly IdempotencyTokenManager _tokenManager;
    private readonly TrueExecutionStrategyFactory<TDbContext> _executionStrategyFactory;
    private readonly IdempotencyTokenRepositoryFactory _idempotencyTokenRepositoryFactory;

    public ExecutionStrategyExtended(IdempotencyTokenManager tokenManager,
        TrueExecutionStrategyFactory<TDbContext> executionStrategyFactory,
        IdempotencyTokenRepositoryFactory idempotencyTokenRepositoryFactory)
    {
        _tokenManager = tokenManager;
        _executionStrategyFactory = executionStrategyFactory;
        _idempotencyTokenRepositoryFactory = idempotencyTokenRepositoryFactory;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        var trueStrategy = _executionStrategyFactory.Create(mainContext);
        return await trueStrategy.ExecuteAsync(action);
    }

    public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel)
    {
        var trueStrategy = _executionStrategyFactory.Create(mainContext);
        return await trueStrategy.ExecuteInTransactionAsync(action, isolationLevel);
    }

    public async Task<TResponse> ExecuteInIdempotentTransactionAsync<TResponse>(
        Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, string idempotencyToken, IsolationLevel isolationLevel)
    {
        var token = _tokenManager.CreateIdempotencyToken(idempotencyToken);

        return await ExecuteInTransactionAsync(async context =>
            {
                var repository = _idempotencyTokenRepositoryFactory.Create(context);
                return await repository.HandleAction(action, token);
            },
            mainContext, isolationLevel);
    }
}