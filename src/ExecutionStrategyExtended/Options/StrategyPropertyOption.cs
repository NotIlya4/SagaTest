namespace ExecutionStrategyExtended.Options;

public class StrategyPropertyOption<TOption> : PropertyOption<TOption, ExecutionStrategyExtendedOptions>
{
    public StrategyPropertyOption(ExecutionStrategyExtendedOptions returnTo) : base(returnTo)
    {
    }
}