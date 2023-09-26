using ExecutionStrategyExtended.Options;
using ExecutionStrategyExtended.ViolationDetector;

namespace ExecutionStrategyExtended.Extensions;

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
        var instance = propertyOption.Value ?? Activator.CreateInstance<TOption>();
        action(instance);
        propertyOption.Value = instance;
        return propertyOption.ReturnTo;
    }
    
    public static TReturn UseDefault<TReturn>(this PropertyOption<IIdempotenceViolationDetector, TReturn> propertyOption, IdempotencyTokenTableOptions tableOptions)
    {
        
    }

    internal static void ThrowIfNoValue<TOption, TReturn>(this PropertyOption<TOption, TReturn> propertyOption, string name)
    {
        if (propertyOption.Value is null)
        {
            throw new InvalidOperationException($"{name} must be provided");
        }
    }
}