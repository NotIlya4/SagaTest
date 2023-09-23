namespace UnitTests.Fixture.PostgresBootstrapper;

public record PostgresConnOptions
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 8888;
    public string Username { get; init; } = "postgres";
    public string Password { get; init; } = "pgpass";
}