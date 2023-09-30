using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.StrategyExtended;

public interface IExecutionStrategyExtended<TDbContext> where TDbContext : DbContext
{
    Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext);

    Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel);

    Task<TResponse> ExecuteInIdempotentTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action, 
        TDbContext mainContext, string idempotencyToken, IsolationLevel isolationLevel);
}