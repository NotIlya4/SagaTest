using Microsoft.EntityFrameworkCore;
using AppContext = EfTest.EntityFramework.AppContext;

public static class EfCoreExtensions
{
    public static void AddAppContext(this IServiceCollection services, string database)
    {
        services.AddDbContext<AppContext>(builder =>
        {
            builder.UseNpgsql($"Host=localhost;Database={database};Port=5432;Username=postgres;Password=pgpass");
        });
    }
}