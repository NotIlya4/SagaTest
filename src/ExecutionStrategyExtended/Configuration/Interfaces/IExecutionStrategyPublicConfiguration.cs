using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.DbContextRetrier;
using ExecutionStrategyExtended.StrategyExtended;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration.Interfaces;

public interface IExecutionStrategyPublicConfiguration
{
    IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClock { get; }
    IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration> DbContextRetrierConfiguration { get; }
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetector { get; }
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializer { get; }
}