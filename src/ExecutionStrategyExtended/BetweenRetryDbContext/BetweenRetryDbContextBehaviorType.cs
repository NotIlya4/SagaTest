namespace ExecutionStrategyExtended.BetweenRetryDbContext;

public enum BetweenRetryDbContextBehaviorType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}