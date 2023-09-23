namespace ExecutionStrategyExtended.Models;

public record IdempotencyToken
{
    public string Id { get; set; } = null!;
    public string Response { get; set; } = null!;
    public DateTime CreateTime { get; set; }

    protected IdempotencyToken() { }

    public IdempotencyToken(string id, string response, DateTime createTime)
    {
        Id = id;
        Response = response;
        CreateTime = createTime;
    }
}