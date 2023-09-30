namespace ExecutionStrategyExtended.BetweenRetryDbContext;

public static class BetweenRetryDbContextBehavior
{
    public static BetweenRetryDbContextBehaviorConfiguration CreateNewDbContextStrategy(bool disposePreviousContext = false)
    {
        return new BetweenRetryDbContextBehaviorConfiguration()
        {
            BetweenRetryDbContextBehaviorType = BetweenRetryDbContextBehaviorType.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static BetweenRetryDbContextBehaviorConfiguration ClearChangeTrackerStrategy()
    {
        return new BetweenRetryDbContextBehaviorConfiguration()
        {
            BetweenRetryDbContextBehaviorType = BetweenRetryDbContextBehaviorType.ClearChangeTracker
        };
    }
    
    public static BetweenRetryDbContextBehaviorConfiguration UseSameDbContextStrategy()
    {
        return new BetweenRetryDbContextBehaviorConfiguration()
        {
            BetweenRetryDbContextBehaviorType = BetweenRetryDbContextBehaviorType.UseSame
        };
    }
}