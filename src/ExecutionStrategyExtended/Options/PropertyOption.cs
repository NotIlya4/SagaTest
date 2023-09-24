namespace ExecutionStrategyExtended.Options;

public class PropertyOption<TOption, TReturnTo>
{
    public TReturnTo ReturnTo { get; }
    public TOption? Value { get; set; } = default;

    public PropertyOption(TReturnTo returnTo)
    {
        ReturnTo = returnTo;
    }
}