using Newtonsoft.Json.Linq;

namespace ExecutionStrategyExtended;

public interface ISerializer
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string rawObj);
}

public class Serializer : ISerializer
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