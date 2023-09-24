using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Options;

public class ExecutionStrategyExtendedOptions
{
    public ExecutionStrategyExtendedOptions()
    {
        SystemClock = new StrategyPropertyOption<ISystemClock>(this);
        ResponseSerializer = new StrategyPropertyOption<IResponseSerializer>(this);
        IdempotencyViolationDetector = new StrategyPropertyOption<IIdempotencyViolationDetector>(this);
        BetweenRetriesBehavior = new StrategyPropertyOption<BetweenRetriesBehaviorOptions>(this);
    }

    public StrategyPropertyOption<ISystemClock> SystemClock { get; }
    public StrategyPropertyOption<IResponseSerializer> ResponseSerializer { get; }
    public StrategyPropertyOption<IIdempotencyViolationDetector> IdempotencyViolationDetector { get; }
    public StrategyPropertyOption<BetweenRetriesBehaviorOptions> BetweenRetriesBehavior { get; }

    internal void Validate()
    {
        SystemClock.ThrowIfNoValue(nameof(SystemClock));
        ResponseSerializer.ThrowIfNoValue(nameof(ResponseSerializer));
        IdempotencyViolationDetector.ThrowIfNoValue(nameof(IdempotencyViolationDetector));
    }
}