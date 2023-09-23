using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using UnitTests.PostgresBootstrapping.Internal;

namespace UnitTests.PostgresBootstrapping;

public class PostgresBootstrapper : IDbBootstrapper
{
    private readonly PostgresConnOptions _options;
    public IDbBootstrapper Bootstrapper { get; }

    public PostgresBootstrapper(PostgresConnOptions options, IDbBootstrapper bootstrapper)
    {
        _options = options;
        Bootstrapper = bootstrapper;
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
        Bootstrapper.Bootstrap();
    }

    public void Destroy()
    {
        Bootstrapper.Destroy();
    }

    public void Clean()
    {
        Bootstrapper.Clean();
    }

    public void Dispose()
    {
        Bootstrapper.Dispose();
    }
}

public class PostgresBootstrapperBuilder
{
    public static PostgresBootstrapperFinalizer BuildForExistingDb()
    {
        var postgresConnOptions = new PostgresConnOptions();
        var delegateBootstrapper = new DelegateDbBootstrapper();
        var postgresBootstrapper = new PostgresBootstrapper(postgresConnOptions, delegateBootstrapper);

        return new PostgresBootstrapperFinalizer(postgresConnOptions, postgresBootstrapper, context =>
        {
            delegateBootstrapper.BootstrapAction = () => context.Database.EnsureCreated();
            delegateBootstrapper.CleanAction = context.CleanTables;
        });
    }

    public static PostgresBootstrapperFinalizer BuildForLocalContainer()
    {
        var postgresConnOptions = new PostgresConnOptions();
        var container = new PostgresContainer(PostgresContainerOptions.FromConnOptions(postgresConnOptions));
        var delegateBootstrapper = new DelegateDbBootstrapper()
        {
            BootstrapAction = () => container.Bootstrap(),
            DestroyAction = () => container.Destroy()
        };
        var postgresBootstrapper = new PostgresBootstrapper(postgresConnOptions, delegateBootstrapper);

        return new PostgresBootstrapperFinalizer(postgresConnOptions, postgresBootstrapper, context =>
        {
            delegateBootstrapper.CleanAction = context.CleanTables;
        });
    }
}

public class PostgresBootstrapperFinalizer
{
    private readonly PostgresConnOptions _options;
    private readonly IDbBootstrapper _bootstrapper;
    private readonly Action<AppDbContext> _finalizeAction;
    
    public PostgresBootstrapperFinalizer(PostgresConnOptions options, IDbBootstrapper bootstrapper, Action<AppDbContext> finalizeAction)
    {
        _options = options;
        _bootstrapper = bootstrapper;
        _finalizeAction = finalizeAction;
    }

    public IDbBootstrapper Finalize(AppDbContext context)
    {
        _finalizeAction(context);
        return _bootstrapper;
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
}