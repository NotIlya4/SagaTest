namespace UnitTests.Fixture;

public record PostgresContainerOptions
{
    public string Image { get; init; } = "postgres:latest";
    public int Port { get; init; } = 8888;
    public string Password { get; init; } = "pgpass";
    public string ContainerName { get; init; } = "postgres-test";

    public static PostgresContainerOptions FromDesiredOptions(DesiredPostgresConnOptions desiredOptions)
    {
        return new PostgresContainerOptions()
        {
            Port = desiredOptions.Port,
            Password = desiredOptions.Password
        };
    }
};