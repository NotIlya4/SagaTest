namespace ExecutionStrategyExtended.Options;

public static class PropertyOptionExtensions 
{
    public static TReturn Use<TOption, TReturn>(this PropertyOption<TOption, TReturn> propertyOption, TOption instance)
    {
        propertyOption.Value = instance;
        return propertyOption.ReturnTo;
    }

    public static TReturn Configure<TOption, TReturn>(this PropertyOption<TOption, TReturn> propertyOption,
        Action<TOption> action)
    {
        var instance = Activator.CreateInstance<TOption>();
        action(instance);
        propertyOption.Value = instance;
        return propertyOption.ReturnTo;
    }
}