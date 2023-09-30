using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended.BetweenRetries;

public class BetweenRetiesStrategyConfiguration
{
    public DbContextRetryPolicy BetweenRetiesPolicy { get; set; }
    public bool DisposePreviousContext { get; set; }
}