namespace UnitTests.Fixture;

public record FluentDockerPostgresOptions
{
    public string Image { get; init; } = "postgres:latest";
    public int Port { get; init; } = 8888;
    public string Password { get; init; } = "pgpass";
    public string ContainerName { get; init; } = "postgres-test";

    public static FluentDockerPostgresOptions FromDesired(DesiredPostgresInstanceOptions desiredOptions)
    {
        return new FluentDockerPostgresOptions()
        {
            Port = desiredOptions.Port,
            Password = desiredOptions.Password
        };
    }
};