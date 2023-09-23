using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UnitTests.Fixture.PostgresBootstrapper;

public class PostgresBootstrapper : IPostgresBootstrapper, IDisposable
{
    private readonly PostgresConnOptions _options;
    private readonly IDbBootstrapper _bootstrapper;

    public PostgresBootstrapper(PostgresConnOptions options, IDbBootstrapper bootstrapper)
    {
        _options = options;
        _bootstrapper = bootstrapper;
    }

    public void ConfigureOptions(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgresConn:Host"] = _options.Host,
                ["PostgresConn:Port"] = _options.Port.ToString(),
                ["PostgresConn:Username"] = _options.Username,
                ["PostgresConn:Password"] = _options.Password,
                ["PostgresConn:Database"] = "Test"
            })
            .Build();
        builder.UseConfiguration(config);
    }

    public void Bootstrap()
    {
        _bootstrapper.Bootstrap();
    }

    public void Destroy()
    {
        _bootstrapper.Destroy();
    }

    public void Clean()
    {
        _bootstrapper.Clean();
    }

    public void Dispose()
    {
        _bootstrapper.Dispose();
    }
}