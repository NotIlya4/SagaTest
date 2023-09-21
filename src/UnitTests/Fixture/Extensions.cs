using Ductus.FluentDocker.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UnitTests.Fixture;

public static class Extensions
{
    public static IWebHostBuilder OverridePostgresPort(this IWebHostBuilder builder, int newPort)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgresConn:Port"] = newPort.ToString()
            })
            .Build();
        builder.UseConfiguration(config);
        return builder;
    }

    public static ContainerBuilder UsePostgresContainer(this ContainerBuilder builder,
        FluentDockerPostgresOptions? options = null)
    {
        options ??= new FluentDockerPostgresOptions();

        return builder
            .WithName(options.ContainerName)
            .DeleteIfExists(true, true)
            .UseImage(options.Image)
            .WithEnvironment($"POSTGRES_PASSWORD={options.Password}")
            .ExposePort(options.Port, 5432)
            .WaitForMessageInLog("database system is ready to accept connections", TimeSpan.FromSeconds(10));
    }
    
    public static ContainerBuilder UsePgAdminContainer(this ContainerBuilder builder,
        FluentDockerPostgresOptions? options = null)
    {
        options ??= new FluentDockerPostgresOptions();

        return builder
            .WithName(options.ContainerName + "-pgadmin")
            .DeleteIfExists(true, true)
            .UseImage(options.PgAdminImage)
            .WithEnvironment($"PGADMIN_DEFAULT_EMAIL={options.PgAdminEmail}", $"PGADMIN_DEFAULT_PASSWORD={options.Password}")
            .ExposePort(options.PgAdminPort, 80);
    }
}