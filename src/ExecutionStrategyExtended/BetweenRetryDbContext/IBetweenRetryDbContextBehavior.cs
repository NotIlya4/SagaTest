using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.BetweenRetries;

internal interface IBetweenRetryDbContextBehavior<TDbContext> where TDbContext : DbContext
{
    IExecutionStrategy CreateExecutionStrategy();
    Task<TDbContext> ProvideDbContextForRetry(int retryNumber);
}