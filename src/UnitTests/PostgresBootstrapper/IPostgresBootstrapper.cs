using Microsoft.AspNetCore.Hosting;

namespace UnitTests.Fixture.PostgresBootstrapper;

public interface IPostgresBootstrapper : IDbBootstrapper
{
    void ConfigureOptions(IWebHostBuilder builder);
}