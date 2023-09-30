using ExecutionStrategyExtended.BetweenRetries;
using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration;

internal interface IExecutionStrategyInternalConfiguration
{
    ISystemClock SystemClock { get; }
    BetweenRetryDbContextBehaviorConfiguration BetweenRetryDbContextBehaviorConfiguration { get; }
    IIdempotenceViolationDetector IdempotenceViolationDetector { get; }
    IResponseSerializer ResponseSerializer { get; }
}