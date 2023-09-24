using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using UnitTests.PostgresBootstrapping.PostgresqlContainer;

namespace UnitTests.PostgresBootstrapping.Bootstrapper;

public enum PostgresBootstrapperType
{
    LocalContainer,
    ExistingDb
}

public class PostgresBootstrapperBuilder
{
    private readonly PostgresBootstrapperType _postgresBootstrapperType;
    private readonly PostgresConnOptions _options;

    public PostgresBootstrapperBuilder(PostgresBootstrapperType postgresBootstrapperType, PostgresConnOptions options)
    {
        _postgresBootstrapperType = postgresBootstrapperType;
        _options = options;
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

    public IDbBootstrapper CreateDbBootstrapper(AppDbContext context)
    {
        return _postgresBootstrapperType switch
        {
            PostgresBootstrapperType.ExistingDb => BuildForExistingDb(context),
            PostgresBootstrapperType.LocalContainer => BuildForLocalContainer(context),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private IDbBootstrapper BuildForExistingDb(AppDbContext context)
    {
        var delegateBootstrapper = new DelegateDbBootstrapper()
        {
            BootstrapAction = () =>
            {
                context.Database.EnsureCreated();
                context.CleanTables();
            },
            CleanAction = context.CleanTables,
            DestroyAction = () => context.Database.EnsureDeleted()
        };

        return delegateBootstrapper;
    }

    private IDbBootstrapper BuildForLocalContainer(AppDbContext context)
    {
        var container = new PostgresContainer(PostgresContainerOptions.FromConnOptions(_options));
        var delegateBootstrapper = new DelegateDbBootstrapper()
        {
            BootstrapAction = () =>
            {
                container.Bootstrap();
                context.EnsureDeletedCreated();
            },
            DestroyAction = () => container.Destroy(),
            CleanAction = context.CleanTables
        };

        return delegateBootstrapper;
    }
}