using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Options;

internal class IdempotentTransactionOptionsConfigure : IConfigureOptions<IdempotentTransactionOptions>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, IdempotentTransactionOptions> _action;

    public IdempotentTransactionOptionsConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, IdempotentTransactionOptions> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(IdempotentTransactionOptions options)
    {
        _action(_serviceProvider, options);
    }
}