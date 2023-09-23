using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Fixture;

public class FluentDockerBootstrapper : IDbBootstrapperCleaner
{
    private readonly PostgresContainerOptions _options;
    private IService? _container;

    public FluentDockerBootstrapper(PostgresContainerOptions options)
    {
        _options = options;
    }

    public void Bootstrap()
    {
        Destroy();
        
        _container = new Builder()
            .UsePostgresContainer(_options)
            .Build().Start();
    }

    public void Destroy()
    {
        _container?.Dispose();
    }

    public void Clean()
    {
        Bootstrap();
    }
}