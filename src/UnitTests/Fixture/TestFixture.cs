using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.Extensions;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace UnitTests.Fixture;

public class TestFixture : IDisposable
{
    private IDbBootstraper _bootstraper;
    private static readonly PostgresContainerOptions PostgresOptions = new();
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }
    public AppContext MainContext { get; }

    public TestFixture()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder
                .OverridePostgresPort(5432)
                .UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        MainContext = Services.CreateScope().ServiceProvider.GetAppContext();
        _bootstraper = new SoftDbContextBootstraper(MainContext);
        
        _bootstraper.PrepareReadyEmptyDb();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public void Dispose()
    {
        _bootstraper.Clear();
        Factory.Dispose();
    }
}