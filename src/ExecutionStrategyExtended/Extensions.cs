using System.Data;
using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public static class Extensions
{
    public static async Task<TResponse> ExecuteInTransactionAsync<TDbContext, TResponse>(
        this TrueExecutionStrategy<TDbContext> trueStrategy, Func<TDbContext, Task<TResponse>> action, 
        IsolationLevel isolationLevel) where TDbContext : DbContext
    {
        return await trueStrategy.ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await action(context);

            await transaction.CommitAsync();

            return response;
        });
    }
    
    public static DbSet<IdempotencyToken> IdempotencyTokens(this DbContext context)
    {
        return context.Set<IdempotencyToken>();
    }

    public static void Detach<T>(this DbContext context, T entity)
    {
        context.Entry(entity!).State = EntityState.Detached;
    }
}