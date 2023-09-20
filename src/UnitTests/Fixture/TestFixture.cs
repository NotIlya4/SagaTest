using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.EntityFramework;
using MoneyService.Extensions;

namespace UnitTests.Fixture;

public class TestFixture : IDisposable
{
    public IDbBootstrapper DbBootstrapper { get; }
    private static readonly PostgresConnectOptions PostgresOptions = new();
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }
    public AppDbContext MainDbContext { get; }

    public TestFixture()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder
                .OverridePostgresPort(5432)
                .UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        MainDbContext = Services.CreateScope().ServiceProvider.GetAppContext();
        DbBootstrapper = new DbContextDbBootstrapper(MainDbContext);
        
        DbBootstrapper.PrepareReadyEmptyDb();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public void Dispose()
    {
        DbBootstrapper.Clear();
        Factory.Dispose();
    }
}