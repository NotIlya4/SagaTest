namespace ExecutionStrategyExtended.Models;

public record IdempotencyToken
{
    public string Id { get; set; } = null!;
    public string Response { get; set; } = null!;
    public DateTimeOffset CreateTime { get; set; }

    protected IdempotencyToken() { }

    public IdempotencyToken(string id, string response, DateTimeOffset createTime)
    {
        Id = id;
        Response = response;
        CreateTime = createTime;
    }
}