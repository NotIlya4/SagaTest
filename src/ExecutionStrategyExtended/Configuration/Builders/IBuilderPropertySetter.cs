namespace ExecutionStrategyExtended.Configuration.Builders;

public interface IBuilderPropertySetter<in TProperty, out TReturn>
{
    TReturn Set(TProperty property);
}