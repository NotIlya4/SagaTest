using ExecutionStrategyExtended.BetweenRetryDbContext;
using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.Configuration.Interfaces;
using ExecutionStrategyExtended.StrategyExtended;
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
        
        BetweenRetryDbContextBehaviorConfigurationBuilder =
            new BuilderProperty<BetweenRetryDbContextBehaviorConfiguration, IExecutionStrategyPublicConfiguration>(
                configuration => BetweenRetryDbContextBehaviorConfiguration = configuration, this,
                BetweenRetryDbContextBehaviorConfiguration);
        
        IdempotenceViolationDetectorBuilder =
            new BuilderProperty<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration>(
                detector => IdempotenceViolationDetector = detector, this);

        ResponseSerializerBuilder = new BuilderProperty<IResponseSerializer, IExecutionStrategyPublicConfiguration>(
            serializer => ResponseSerializer = serializer, this);
    }
    
    public IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClockBuilder { get; }
    public IBuilderPropertySetterConfig<BetweenRetryDbContextBehaviorConfiguration, IExecutionStrategyPublicConfiguration> BetweenRetryDbContextBehaviorConfigurationBuilder { get; }
    public IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetectorBuilder { get; }
    public IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializerBuilder { get; }

    public ISystemClock SystemClock { get; set; } = new SystemClock();
    public BetweenRetryDbContextBehaviorConfiguration BetweenRetryDbContextBehaviorConfiguration { get; set; } = new();
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