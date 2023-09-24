using OneOf;
using OneOf.Types;

namespace ExecutionStrategyExtended;

[GenerateOneOf]
internal partial class IdempotencyTokenAddResult : OneOfBase<Success, AlreadyExists>
{
    public bool IsAlreadyExists()
    {
        return Value is AlreadyExists;
    }
}

internal struct AlreadyExists
{
    public Exception Exception { get; }

    public AlreadyExists(Exception exception)
    {
        Exception = exception;
    }
}