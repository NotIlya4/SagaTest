namespace ExecutionStrategyExtended.Configuration.Builders;

internal class BuilderProperty<TProperty, TReturn> : IBuilderPropertySetterConfig<TProperty, TReturn>
{
    private readonly Action<TProperty> _onSet;
    private readonly TReturn _returnTo;
    private readonly TProperty? _property;

    public BuilderProperty(Action<TProperty> onSet, TReturn returnTo, TProperty? property = default)
    {
        _property = property;
        _onSet = onSet;
        _returnTo = returnTo;
    }

    public TReturn Set(TProperty property)
    {
        _onSet(property);
        return _returnTo;
    }

    public TReturn Config(Action<TProperty> action)
    {
        action(_property!);
        return _returnTo;
    }
}