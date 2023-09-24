using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace ExecutionStrategyExtended.Options;

internal class ExecutionStrategyExtendedOptionsConfigure : IConfigureOptions<ExecutionStrategyExtendedOptions>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, ExecutionStrategyExtendedOptions> _action;

    public ExecutionStrategyExtendedOptionsConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, ExecutionStrategyExtendedOptions> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedOptions options)
    {
        options.SystemClock.Use(new SystemClock());
        options.BetweenRetriesBehavior.Use(new BetweenRetriesBehaviorOptions());
        options.IdempotencyTokenTableOptions.Use(new IdempotencyTokenTableOptions());
        
        _action(_serviceProvider, options);
    }
}