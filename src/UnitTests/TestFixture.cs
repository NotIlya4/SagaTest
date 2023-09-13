using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using AppContext = EfTest.EntityFramework.AppContext;

namespace UnitTests;

public class TestFixture : IDisposable
{
    internal WebApplicationFactory<Program> Factory { get; }
    public AppContext Context { get; }
    public IServiceProvider Services { get; }

    public TestFixture()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
        Services = Factory.Services;

        Context = Services.CreateScope().ServiceProvider.GetRequiredService<AppContext>();
        
        ReloadDb();
    }

    public void ReloadDb()
    {
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Factory.Dispose();
    }
}