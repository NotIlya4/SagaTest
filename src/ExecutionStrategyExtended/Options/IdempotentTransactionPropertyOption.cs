namespace ExecutionStrategyExtended.Options;

public class IdempotentTransactionPropertyOption<TOption> : PropertyOption<TOption, IdempotentTransactionOptions>
{
    public IdempotentTransactionPropertyOption(IdempotentTransactionOptions returnTo) : base(returnTo)
    {
    }
}