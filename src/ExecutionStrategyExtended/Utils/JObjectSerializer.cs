using Newtonsoft.Json.Linq;

namespace ExecutionStrategyExtended.Utils;

public class JObjectSerializer : IResponseSerializer
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