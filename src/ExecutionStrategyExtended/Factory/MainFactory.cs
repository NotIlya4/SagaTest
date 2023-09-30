using ExecutionStrategyExtended.Configuration.Interfaces;

namespace ExecutionStrategyExtended.Factory;

internal class MainFactory
{
    public MainFactory(IExecutionStrategyInternalConfiguration configuration, IServiceProvider provider)
    {
        IdempotencyToken = new IdempotencyTokenFactoryPart(configuration);
        ExecutionStrategy = new ExecutionStrategyFactoryPart(configuration, provider);
    }
    
    public IdempotencyTokenFactoryPart IdempotencyToken { get; }
    public ExecutionStrategyFactoryPart ExecutionStrategy { get; }
}