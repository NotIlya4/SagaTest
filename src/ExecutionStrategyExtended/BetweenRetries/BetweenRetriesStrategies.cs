using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended.BetweenRetries;

public static class BetweenRetriesStrategies
{
    public static BetweenRetiesStrategyConfiguration CreateNewDbContextStrategy(bool disposePreviousContext = false)
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static BetweenRetiesStrategyConfiguration ClearChangeTrackerStrategy()
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.ClearChangeTracker
        };
    }
    
    public static BetweenRetiesStrategyConfiguration UseSameDbContextStrategy()
    {
        return new BetweenRetiesStrategyConfiguration()
        {
            BetweenRetiesPolicy = DbContextRetryPolicy.UseSame
        };
    }
}