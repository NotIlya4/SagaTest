namespace ExecutionStrategyExtended.Models;

public enum BetweenRetryDbContextBehaviorType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}