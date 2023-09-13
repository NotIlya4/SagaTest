namespace EfTest.Models;

public record Idempotency
{
    public string Id { get; private set; } = null!;

    protected Idempotency() { }

    public Idempotency(string id)
    {
        Id = id;
    }
}