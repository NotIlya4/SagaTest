using ExecutionStrategyExtended.StrategyExtended;
using Newtonsoft.Json.Linq;

namespace MoneyService.ExtendedExecutionStrategyImpls;

public class ResponseSerializer : IResponseSerializer
{
    public string Serialize<T>(T obj)
    {
        return JObject.FromObject(obj!).ToString();
    }

    public T Deserialize<T>(string rawObj)
    {
        return JObject.Parse(rawObj).ToObject<T>()!;
    }
}