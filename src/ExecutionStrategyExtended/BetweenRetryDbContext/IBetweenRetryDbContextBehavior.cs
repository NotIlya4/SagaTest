using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.BetweenRetryDbContext;

internal interface IBetweenRetryDbContextBehavior<TDbContext> where TDbContext : DbContext
{
    IExecutionStrategy CreateExecutionStrategy();
    Task<TDbContext> ProvideDbContextForRetry(int retryNumber);
}