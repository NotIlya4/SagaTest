using OneOf;
using OneOf.Types;

namespace ExecutionStrategyExtended;

[GenerateOneOf]
public partial class IdempotencyTokenAddResult : OneOfBase<Success, AlreadyExists>
{
    public bool IsAlreadyExists()
    {
        return Value is AlreadyExists;
    }
}

public struct AlreadyExists
{
    public Exception Exception { get; }

    public AlreadyExists(Exception exception)
    {
        Exception = exception;
    }
}