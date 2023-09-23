using Ductus.FluentDocker.Builders;
using UnitTests.PostgresBootstrapping.Internal;

namespace UnitTests;

public static class Extensions
{

    public static ContainerBuilder UsePostgresContainer(this Builder builder,
        PostgresContainerOptions? options = null)
    {
        options ??= new PostgresContainerOptions();

        return builder
            .UseContainer()
            .WithName(options.ContainerName)
            .DeleteIfExists(true, true)
            .UseImage(options.Image)
            .WithEnvironment($"POSTGRES_PASSWORD={options.Password}")
            .ExposePort(options.Port, 5432)
            .WaitForMessageInLog("database system is ready to accept connections", TimeSpan.FromSeconds(10));
    }
}