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
    private static readonly DesiredPostgresInstanceOptions PostgresOptions = new();
    public IDbBootstrapper DbBootstrapper { get; }
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }
    public AppDbContext MainDbContext { get; }

    public TestFixture()
    {
        var manifest = new InfrastructureManifest(PostgresOptions);
        
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            manifest.ConfigureOptions(builder);
            builder.UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        MainDbContext = Services.CreateScope().ServiceProvider.GetAppContext();
        DbBootstrapper = new DbContextDbBootstrapper(MainDbContext);
        
        DbBootstrapper.Bootstrap();
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