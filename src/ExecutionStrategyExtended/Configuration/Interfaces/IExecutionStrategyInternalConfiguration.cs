using ExecutionStrategyExtended.BetweenRetryDbContext;
using ExecutionStrategyExtended.StrategyExtended;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration.Interfaces;

internal interface IExecutionStrategyInternalConfiguration
{
    ISystemClock SystemClock { get; }
    BetweenRetryDbContextBehaviorConfiguration BetweenRetryDbContextBehaviorConfiguration { get; }
    IIdempotenceViolationDetector IdempotenceViolationDetector { get; }
    IResponseSerializer ResponseSerializer { get; }
}