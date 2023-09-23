using Ductus.FluentDocker.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UnitTests.Fixture;

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