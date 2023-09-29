namespace ExecutionStrategyExtended.Configuration.Builders;

public interface IBuilderPropertyConfig<out TProperty, out TReturn>
{
    TReturn Config(Action<TProperty> action);
}