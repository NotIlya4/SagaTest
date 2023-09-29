namespace ExecutionStrategyExtended.Configuration.Builders;

public interface IBuilderPropertySetterConfig<TProperty, out TReturn> : IBuilderPropertySetter<TProperty, TReturn>, IBuilderPropertyConfig<TProperty, TReturn>
{
    
}