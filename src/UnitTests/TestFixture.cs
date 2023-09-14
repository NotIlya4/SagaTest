using EfTest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace UnitTests;

public class TestFixture : IDisposable
{
    internal WebApplicationFactory<Program> Factory { get; }
    public IServiceProvider Services { get; }

    public TestFixture()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
        Services = Factory.Services;

        Services.CreateScope().ServiceProvider.GetAppContext().ReloadDb();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public void Dispose()
    {
        Factory.Dispose();
    }
}