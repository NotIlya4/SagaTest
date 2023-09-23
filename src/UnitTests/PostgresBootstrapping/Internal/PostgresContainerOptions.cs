namespace UnitTests.PostgresBootstrapping.Internal;

public record PostgresContainerOptions
{
    public string Image { get; init; } = "postgres:latest";
    public int Port { get; init; } = 8888;
    public string Password { get; init; } = "pgpass";
    public string ContainerName { get; init; } = "postgres-test";

    public static PostgresContainerOptions FromConnOptions(PostgresConnOptions options)
    {
        return new PostgresContainerOptions()
        {
            Port = options.Port,
            Password = options.Password
        };
    }
};