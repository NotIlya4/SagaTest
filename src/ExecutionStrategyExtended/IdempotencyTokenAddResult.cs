namespace ExecutionStrategyExtended;

internal class IdempotencyTokenAddResult
{
    public bool IsAlreadyExists { get; }

    public IdempotencyTokenAddResult(bool isAlreadyExists)
    {
        IsAlreadyExists = isAlreadyExists;
    }
}