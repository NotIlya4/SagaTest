namespace UnitTests;

public record PostgresContainerOptions
{
    public int Port { get; init; } = 8888;
    public string Image { get; init; } = "postgres:latest";
    public string Password { get; init; } = "pgpass";
    public string ContainerName { get; init; } = "postgres-test";
};