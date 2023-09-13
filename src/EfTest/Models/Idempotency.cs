namespace EfTest.Models;

public record Idempotency
{
    public string Id { get; private set; } = null!;

    protected Idempotency() { }

    public Idempotency(string id)
    {
        Id = id;
    }

    public static implicit operator Idempotency(string id) => new Idempotency(id);
    public static implicit operator string(Idempotency idempotency) => idempotency.Id;
}