namespace ExecutionStrategyExtended.Configuration;

internal class MainFactory
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;
    private readonly IServiceProvider _provider;

    public MainFactory(IExecutionStrategyInternalConfiguration configuration, IServiceProvider provider)
    {
        _configuration = configuration;
        _provider = provider;
        IdempotencyToken = new IdempotencyTokenFactoryPart(configuration, provider);
        ExecutionStrategy = new ExecutionStrategyFactoryPart(configuration, provider);
    }
    
    public IdempotencyTokenFactoryPart IdempotencyToken { get; }
    public ExecutionStrategyFactoryPart ExecutionStrategy { get; }
}