using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Fixture;

public class FluentDockerDbBootstrapper : IDbBootstrapper, IDbCleaner
{
    private readonly FluentDockerPostgresOptions _fluentDockerPostgresOptions;
    private IService? _container;

    public FluentDockerDbBootstrapper(FluentDockerPostgresOptions fluentDockerPostgresOptions)
    {
        _fluentDockerPostgresOptions = fluentDockerPostgresOptions;
    }

    public void Bootstrap()
    {
        Destroy();
        
        _container = new Builder()
            .UseContainer().UsePostgresContainer()
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