using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.Extensions;

namespace UnitTests.Fixture;

public class TestFixture : IDisposable
{
    private readonly IContainerService _container;
    private static readonly PostgresContainerOptions PostgresOptions = new();
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }

    public TestFixture()
    {
        _container = new Builder()
            .UsePostgresContainer(PostgresOptions)
            .Build()
            .Start();
    
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder
                .OverridePostgresPort(PostgresOptions.Port)
                .UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        Services.CreateScope().ServiceProvider.GetAppContext().ReloadDb();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public void Dispose()
    {
        _container.Dispose();
        Factory.Dispose();
    }
}