﻿using Ductus.FluentDocker.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UnitTests;

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

    public static ContainerBuilder UsePostgresContainer(this Builder builder,
        PostgresContainerOptions? options = null)
    {
        options ??= new PostgresContainerOptions();

        return builder
            .UseContainer()
            .UsePostgresContainer(options);
    }

    public static ContainerBuilder UsePostgresContainer(this ContainerBuilder builder,
        PostgresContainerOptions? options = null)
    {
        options ??= new PostgresContainerOptions();

        return builder
            .WithName(options.ContainerName)
            .DeleteIfExists(true, true)
            .UseImage(options.Image)
            .WithEnvironment($"POSTGRES_PASSWORD={options.Password}")
            .ExposePort(options.Port, 5432)
            .WaitForMessageInLog("database system is ready to accept connections", TimeSpan.FromSeconds(10));
    }
}