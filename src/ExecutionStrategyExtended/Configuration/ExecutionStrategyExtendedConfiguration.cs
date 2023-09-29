using ExecutionStrategyExtended.BetweenRetries;
using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration;

internal record ExecutionStrategyExtendedConfiguration : IExecutionStrategyPublicConfiguration, IExecutionStrategyInternalConfiguration
{
    private IIdempotenceViolationDetector? _idempotenceViolationDetector;
    private IResponseSerializer? _responseSerializer;

    public ExecutionStrategyExtendedConfiguration()
    {
        SystemClockBuilder =
            new BuilderProperty<ISystemClock, IExecutionStrategyPublicConfiguration>(clock => SystemClock = clock, this,
                SystemClock);
        
        BetweenRetriesBehaviorConfigurationBuilder =
            new BuilderProperty<BetweenRetriesBehaviorConfiguration, IExecutionStrategyPublicConfiguration>(
                configuration => BetweenRetriesBehaviorConfiguration = configuration, this,
                BetweenRetriesBehaviorConfiguration);
        
        IdempotenceViolationDetectorBuilder =
            new BuilderProperty<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration>(
                detector => IdempotenceViolationDetector = detector, this);

        ResponseSerializerBuilder = new BuilderProperty<IResponseSerializer, IExecutionStrategyPublicConfiguration>(
            serializer => ResponseSerializer = serializer, this);
    }
    
    public IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClockBuilder { get; }
    public IBuilderPropertySetterConfig<BetweenRetriesBehaviorConfiguration, IExecutionStrategyPublicConfiguration> BetweenRetriesBehaviorConfigurationBuilder { get; }
    public IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetectorBuilder { get; }
    public IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializerBuilder { get; }

    public ISystemClock SystemClock { get; set; } = new SystemClock();
    public BetweenRetriesBehaviorConfiguration BetweenRetriesBehaviorConfiguration { get; set; } = new();
    public IIdempotenceViolationDetector IdempotenceViolationDetector
    {
        get => _idempotenceViolationDetector!;
        set => _idempotenceViolationDetector = value;
    }
    public IResponseSerializer ResponseSerializer
    {
        get => _responseSerializer!;
        set => _responseSerializer = value;
    }
}