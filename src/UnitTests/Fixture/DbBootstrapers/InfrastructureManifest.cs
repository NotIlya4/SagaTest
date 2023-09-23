using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;

namespace UnitTests.Fixture;

public class InfrastructureManifest
{
    private readonly DesiredPostgresState _postgresState;

    public InfrastructureManifest(DesiredPostgresState postgresState)
    {
        _postgresState = postgresState;
    }

    public void ConfigureOptions(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgresConn:Host"] = _postgresState.PostgresOptions.Host,
                ["PostgresConn:Port"] = _postgresState.PostgresOptions.Port.ToString(),
                ["PostgresConn:Username"] = _postgresState.PostgresOptions.Username,
                ["PostgresConn:Password"] = _postgresState.PostgresOptions.Password,
                ["PostgresConn:Database"] = "Test"
            })
            .Build();
        builder.UseConfiguration(config);
    }

    public IDbBootstrapper CreateBootstrapper(AppDbContext context)
    {
        return _postgresState.BootstrapPolicy switch
        {
            PostgresBootstrapPolicy.ExistingContainer => new FluentDockerDbBootstrapper(
                FluentDockerPostgresOptions.FromDesired(_postgresState.PostgresOptions)),
            PostgresBootstrapPolicy.LocalContainer => new DbContextDbBootstrapper(context,
                _postgresState.CleanupPolicy == PostgresCleanupPolicy.Soft),
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

public record DesiredPostgresState
{
    public required DesiredPostgresInstanceOptions PostgresOptions { get; set; }
    public required PostgresCleanupPolicy CleanupPolicy { get; set; }
    public required PostgresBootstrapPolicy BootstrapPolicy { get; set; }
}