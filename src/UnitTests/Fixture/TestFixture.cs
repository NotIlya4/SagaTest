using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using UnitTests.PostgresBootstrapping;
using UnitTests.PostgresBootstrapping.Bootstrapper;

namespace UnitTests.Fixture;

public class TestFixture : IDisposable
{
    public IDbBootstrapper Bootstrapper { get; }
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }
    public AppDbContext MainDbContext { get; }

    public TestFixture()
    {
        var bootstrapperBuilder = new PostgresBootstrapperBuilder(PostgresBootstrapperType.ExistingDb, new PostgresConnOptions());
        
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            bootstrapperBuilder.ConfigureOptions(builder);
            builder.UseEnvironment(AppEnvironments.Test);
        });
        Services = Factory.Services;

        MainDbContext = Services.CreateScope().ServiceProvider.GetAppContext();
        Bootstrapper = bootstrapperBuilder.CreateDbBootstrapper(MainDbContext);
        
        Bootstrapper.Bootstrap();
        Bootstrapper.Clean();
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