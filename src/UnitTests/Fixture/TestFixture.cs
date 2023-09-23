using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using UnitTests.Fixture.PostgresBootstrapper;

namespace UnitTests.Fixture;

public class TestFixture : IDisposable
{
    private static readonly PostgresConnOptions PostgresOptions = new();
    public PostgresBootstrapper.PostgresBootstrapper Bootstrapper { get; }
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }
    public AppDbContext MainDbContext { get; }

    public TestFixture()
    {
        var postgresContainer = new PostgresContainer(PostgresContainerOptions.FromConnOptions(PostgresOptions));
        
        var dbBootstrapper = new DelegateDbBootstrapper()
        {
            BootstrapAction = () => postgresContainer.Bootstrap(),
            DestroyAction = () => postgresContainer.Destroy()
        };
        Bootstrapper = new PostgresBootstrapper.PostgresBootstrapper(PostgresOptions, dbBootstrapper);
        
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            Bootstrapper.ConfigureOptions(builder);
            builder.UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        MainDbContext = Services.CreateScope().ServiceProvider.GetAppContext();
        dbBootstrapper.CleanAction = () => MainDbContext.EnsureDeletedCreated();
        
        Bootstrapper.Bootstrap();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public void Dispose()
    {
        Bootstrapper.Dispose();
        Factory.Dispose();
    }
}