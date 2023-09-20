using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Fixture;

public class FluentDockerDbBootstrapper : IDbBootstrapper
{
    private readonly PostgresConnectOptions _postgresOptions;
    private IService? _container;

    public FluentDockerDbBootstrapper(PostgresConnectOptions postgresOptions)
    {
        _postgresOptions = postgresOptions;
    }

    public void PrepareReadyEmptyDb()
    {
        if (_container is not null)
        {
            _container.Dispose();
        }
        
        _container = new Builder()
            .UseContainer().UsePostgresContainer()
            .Build().Start();
    }

    public void Clear()
    {
        _container?.Dispose();
    }
}