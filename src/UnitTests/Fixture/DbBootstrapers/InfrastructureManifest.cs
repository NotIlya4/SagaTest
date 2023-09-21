using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;

namespace UnitTests.Fixture;

public class InfrastructureManifest
{
    private readonly DesiredPostgresOptions _postgresOptions;

    public InfrastructureManifest(DesiredPostgresOptions postgresOptions)
    {
        _postgresOptions = postgresOptions;
    }

    public void ConfigureOptions(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgresConn:Host"] = _postgresOptions.PostgresOptions.Host,
                ["PostgresConn:Port"] = _postgresOptions.PostgresOptions.Port.ToString(),
                ["PostgresConn:Username"] = _postgresOptions.PostgresOptions.Username,
                ["PostgresConn:Password"] = _postgresOptions.PostgresOptions.Password,
                ["PostgresConn:Database"] = "Test"
            })
            .Build();
        builder.UseConfiguration(config);
    }

    public IDbBootstrapper CreateBootstrapper(AppDbContext context)
    {
        return _postgresOptions.BootstrapPolicy switch
        {
            PostgresBootstrapPolicy.ExistingContainer => new FluentDockerDbBootstrapper(
                FluentDockerPostgresOptions.FromDesired(_postgresOptions.PostgresOptions)),
            PostgresBootstrapPolicy.LocalContainer => new DbContextDbBootstrapper(context,
                _postgresOptions.CleanupPolicy == PostgresCleanupPolicy.Soft),
            _ => throw new NotImplementedException()
        };
    }
}

public record DesiredPostgresInstanceOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8888;
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
}

public enum PostgresCleanupPolicy
{
    Hard,
    Soft
}

public enum PostgresBootstrapPolicy
{
    LocalContainer,
    ExistingContainer
}

public record DesiredPostgresOptions
{
    public DesiredPostgresInstanceOptions PostgresOptions { get; set; }
    public PostgresCleanupPolicy CleanupPolicy { get; set; }
    public PostgresBootstrapPolicy BootstrapPolicy { get; set; }
}