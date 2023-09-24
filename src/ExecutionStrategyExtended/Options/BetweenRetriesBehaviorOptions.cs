using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended.Options;

public class BetweenRetriesBehaviorOptions
{ 
    public DbContextRetryPolicy RetryPolicy { get; set; } = DbContextRetryPolicy.CreateNew;
    public bool DisposeContextOnCreateNew { get; set; } = false;

    public BetweenRetriesBehaviorOptions WithRetryPolicy(DbContextRetryPolicy retryPolicy)
    {
        RetryPolicy = retryPolicy;
        return this;
    }
    
    public BetweenRetriesBehaviorOptions WithDisposeContextOnCreateNew(bool disposeContext)
    {
        DisposeContextOnCreateNew = disposeContext;
        return this;
    }
}