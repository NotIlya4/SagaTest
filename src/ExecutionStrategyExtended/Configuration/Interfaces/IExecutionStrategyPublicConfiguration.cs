using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.DbContextRetrier;
using ExecutionStrategyExtended.StrategyExtended;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration.Interfaces;

public interface IExecutionStrategyPublicConfiguration
{
    IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClockBuilder { get; }
    IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration> BetweenRetryDbContextBehaviorConfigurationBuilder { get; }
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetectorBuilder { get; }
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializerBuilder { get; }
}