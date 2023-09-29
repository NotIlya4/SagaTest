using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended.BetweenRetries;

public class BetweenRetriesBehaviorConfiguration
{ 
    public DbContextRetryPolicy RetryPolicy { get; set; } = DbContextRetryPolicy.CreateNew;
    public bool DisposeContextOnCreateNew { get; set; }

    public BetweenRetriesBehaviorConfiguration WithRetryPolicy(DbContextRetryPolicy retryPolicy)
    {
        RetryPolicy = retryPolicy;
        return this;
    }
    
    public BetweenRetriesBehaviorConfiguration WithDisposeContextOnCreateNew(bool disposeContext)
    {
        DisposeContextOnCreateNew = disposeContext;
        return this;
    }
}