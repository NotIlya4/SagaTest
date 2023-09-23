using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;
using OneOf;

namespace UnitTests.Fixture;

public class InfrastructureManifest : IDbBootstrapperCleaner
{
    private readonly PostgresManifestOptions _options;
    private readonly Lazy<FluentDockerBootstrapper> _fluentDockerBootstrapper;
    private readonly Lazy<HardDbContextBootstrapper> _hardDbContextBootstrapper;
    private readonly Lazy<SoftDbContextBootstrapper> _softDbContextBootstrapper;
    private readonly Lazy<IDbBootstrapper> _bootstrapper;
    private readonly Lazy<IDbCleaner> _cleaner;
    private AppDbContext? _context;

    public InfrastructureManifest(PostgresManifestOptions options)
    {
        _options = options;

        _fluentDockerBootstrapper = new Lazy<FluentDockerBootstrapper>(() =>
            new FluentDockerBootstrapper(PostgresContainerOptions.FromDesiredOptions(options.PostgresOptions)));
        _hardDbContextBootstrapper =
            new Lazy<HardDbContextBootstrapper>(() => new HardDbContextBootstrapper(_context!));
        _softDbContextBootstrapper =
            new Lazy<SoftDbContextBootstrapper>(() => new SoftDbContextBootstrapper(_context!));
    }

    public void WithContext(AppDbContext context)
    {
        _context = context;
    } 

    public void ConfigureOptions(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgresConn:Host"] = _options.PostgresOptions.Host,
                ["PostgresConn:Port"] = _options.PostgresOptions.Port.ToString(),
                ["PostgresConn:Username"] = _options.PostgresOptions.Username,
                ["PostgresConn:Password"] = _options.PostgresOptions.Password,
                ["PostgresConn:Database"] = "Test"
            })
            .Build();
        builder.UseConfiguration(config);
    }
    
    public void Bootstrap()
    {
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    public void Clean()
    {
        throw new NotImplementedException();
    }
}

public record DesiredPostgresConnOptions
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 8888;
    public string Username { get; init; } = "postgres";
    public string Password { get; init; } = "pgpass";
}

public enum PostgresCleanupPolicy
{
    Hard,
    Soft,
    RecreateContainer
}

public enum PostgresBootstrapPolicy
{
    LocalContainer,
    ExistingContainer
}

public record PostgresManifestOptions
{
    public required DesiredPostgresConnOptions PostgresOptions { get; init; }
    public required PostgresCleanupPolicy CleanupPolicy { get; init; }
    public required PostgresBootstrapPolicy BootstrapPolicy { get; init; }
}