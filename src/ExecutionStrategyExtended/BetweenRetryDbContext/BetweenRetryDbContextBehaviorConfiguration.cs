using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended.BetweenRetries;

public class BetweenRetryDbContextBehaviorConfiguration
{
    public BetweenRetryDbContextBehaviorType BetweenRetryDbContextBehaviorType { get; set; }
    public bool DisposePreviousContext { get; set; }
}