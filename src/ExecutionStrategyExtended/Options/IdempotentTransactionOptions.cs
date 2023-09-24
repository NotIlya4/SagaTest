using ExecutionStrategyExtended.Utils;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Options;

public class IdempotentTransactionOptions
{
    public IdempotentTransactionOptions()
    {
        SystemClock = new IdempotentTransactionPropertyOption<ISystemClock>(this);
        SystemClock.Value = new SystemClock();
        ResponseSerializer = new IdempotentTransactionPropertyOption<IResponseSerializer>(this);
        IdempotencyViolationDetector = new IdempotentTransactionPropertyOption<IIdempotencyViolationDetector>(this);
        BetweenRetriesBehavior = new IdempotentTransactionPropertyOption<BetweenRetriesBehaviorOptions>(this);
    }

    public IdempotentTransactionPropertyOption<ISystemClock> SystemClock { get; }
    public IdempotentTransactionPropertyOption<IResponseSerializer> ResponseSerializer { get; }
    public IdempotentTransactionPropertyOption<IIdempotencyViolationDetector> IdempotencyViolationDetector { get; }
    public IdempotentTransactionPropertyOption<BetweenRetriesBehaviorOptions> BetweenRetriesBehavior { get; }

    internal void Validate()
    {
        SystemClock.ThrowIfNoValue(nameof(SystemClock));
        ResponseSerializer.ThrowIfNoValue(nameof(ResponseSerializer));
        IdempotencyViolationDetector.ThrowIfNoValue(nameof(IdempotencyViolationDetector));
    }
}