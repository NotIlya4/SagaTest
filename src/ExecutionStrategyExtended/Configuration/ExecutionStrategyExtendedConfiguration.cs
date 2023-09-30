using ExecutionStrategyExtended.Configuration.Builders;
using ExecutionStrategyExtended.Configuration.Interfaces;
using ExecutionStrategyExtended.DbContextRetrier;
using ExecutionStrategyExtended.StrategyExtended;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Configuration;

internal record ExecutionStrategyExtendedConfiguration : IExecutionStrategyPublicConfiguration, IExecutionStrategyInternalConfiguration
{
    private IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> _systemClockBuilder;
    private IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration> _dbContextRetrierConfigurationBuilder;
    private IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> _idempotenceViolationDetectorBuilder;
    private IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> _responseSerializerBuilder;
    private ISystemClock _systemClock = new SystemClock();
    private DbContextRetrierConfiguration _dbContextRetrierConfiguration = new();
    private IIdempotenceViolationDetector? _idempotenceViolationDetector;
    private IResponseSerializer? _responseSerializer;

    public ExecutionStrategyExtendedConfiguration()
    {
        _systemClockBuilder =
            new BuilderProperty<ISystemClock, IExecutionStrategyPublicConfiguration>(clock => _systemClock = clock, this,
                _systemClock);
        
        _dbContextRetrierConfigurationBuilder =
            new BuilderProperty<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration>(
                configuration => _dbContextRetrierConfiguration = configuration, this,
                _dbContextRetrierConfiguration);
        
        _idempotenceViolationDetectorBuilder =
            new BuilderProperty<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration>(
                detector => _idempotenceViolationDetector = detector, this);

        _responseSerializerBuilder = new BuilderProperty<IResponseSerializer, IExecutionStrategyPublicConfiguration>(
            serializer => _responseSerializer = serializer, this);
    }


    ISystemClock IExecutionStrategyInternalConfiguration.SystemClock => _systemClock;
    DbContextRetrierConfiguration IExecutionStrategyInternalConfiguration.DbContextRetrierConfiguration => _dbContextRetrierConfiguration;
    IIdempotenceViolationDetector? IExecutionStrategyInternalConfiguration.IdempotenceViolationDetector => _idempotenceViolationDetector;
    IResponseSerializer IExecutionStrategyInternalConfiguration.ResponseSerializer => _responseSerializer!;

    IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> IExecutionStrategyPublicConfiguration.SystemClock => _systemClockBuilder;
    IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration> IExecutionStrategyPublicConfiguration.DbContextRetrierConfiguration => _dbContextRetrierConfigurationBuilder;
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IExecutionStrategyPublicConfiguration.IdempotenceViolationDetector => _idempotenceViolationDetectorBuilder;
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> IExecutionStrategyPublicConfiguration.ResponseSerializer => _responseSerializerBuilder;
}