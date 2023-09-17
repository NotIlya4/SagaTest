using System.Data;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace MoneyService.IdempotentTransactions;

public static class IdempotentTransactionsExtensions
{
    private class State { }

    public static async Task<TResponse> ExecuteInTransactionAsync<TResponse>(this IExecutionStrategy strategy,
        Func<Task<TResponse>> action, IsolationLevel isolationLevel)
    {
        // return await ExecutionStrategyExtensions.ExecuteInTransactionAsync(
        //     strategy,
        //     new State(),
        //     async (_, _) =>
        //     {
        //         return await action();
        //     },
        //     (_, _) => Task.FromResult(false),
        //     (c, _) => c.Database.BeginTransactionAsync(isolationLevel),
        //     CancellationToken.None);
        
        return await strategy.ExecuteAsync<State>(
            new State(),
            async (a, b) =>
            {
                
            },
            )
    }

    public static DbSet<IdempotencyToken> IdempotencyTokens(this DbContext context)
    {
        return context.Set<IdempotencyToken>();
    }

    public static Task<IdempotencyToken> GetIdempotencyToken(this IQueryable<IdempotencyToken> idempotencyTokens, string idempotencyToken)
    {
        return idempotencyTokens.SingleAsync(i => i.Id == idempotencyToken);
    }

    public static Task<IdempotencyToken> GetIdempotencyToken(this DbContext dbContext, string idempotencyToken)
    {
        return dbContext.IdempotencyTokens().GetIdempotencyToken(idempotencyToken);
    }

    public static void DetachIdempotencyToken(this DbContext context, IdempotencyToken idempotencyToken)
    {
        context.Entry(idempotencyToken).State = EntityState.Detached;
    }
}