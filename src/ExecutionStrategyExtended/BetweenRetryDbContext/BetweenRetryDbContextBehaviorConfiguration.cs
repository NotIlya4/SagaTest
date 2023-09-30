namespace ExecutionStrategyExtended.BetweenRetryDbContext;

public class BetweenRetryDbContextBehaviorConfiguration
{
    public BetweenRetryDbContextBehaviorType BetweenRetryDbContextBehaviorType { get; set; }
    public bool DisposePreviousContext { get; set; }
}