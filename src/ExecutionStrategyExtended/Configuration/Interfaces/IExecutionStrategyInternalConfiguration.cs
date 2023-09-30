using ExecutionStrategyExtended.DbContextRetrier;
using ExecutionStrategyExtended.StrategyExtended;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration.Interfaces;

internal interface IExecutionStrategyInternalConfiguration
{
    ISystemClock SystemClock { get; }
    DbContextRetrierConfiguration DbContextRetrierConfiguration { get; }
    IIdempotenceViolationDetector IdempotenceViolationDetector { get; }
    IResponseSerializer ResponseSerializer { get; }
}