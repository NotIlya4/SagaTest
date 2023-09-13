namespace EfTest.Models;

public class Idempotency
{
    public string Id { get; private set; }

    protected Idempotency()
    {
        Id = null!;
    }

    public Idempotency(string id)
    {
        Id = id;
    }
}