using ExecutionStrategyExtended.BetweenRetries;
using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration;

public interface IExecutionStrategyPublicConfiguration
{
    IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClockBuilder { get; }
    IBuilderPropertySetterConfig<BetweenRetryDbContextBehaviorConfiguration, IExecutionStrategyPublicConfiguration> BetweenRetryDbContextBehaviorConfigurationBuilder { get; }
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetectorBuilder { get; }
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializerBuilder { get; }
}