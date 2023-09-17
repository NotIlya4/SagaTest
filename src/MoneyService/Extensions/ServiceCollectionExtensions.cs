using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace MoneyService.Extensions;

public static class ServiceCollectionExtensions
{
    public static string GetPostgresConn(this IConfiguration config)
    {
        return config.GetSection("PostgresConn").Get<NpgsqlConnectionStringBuilder>()!.ToString();
    }
    
    public static void AddAppContext(this IServiceCollection services, string conn)
    {
        services.AddDbContextFactory<AppContext>(builder =>
        {
            builder.ConfigureAppContext(conn);
        });
        services.AddDbContext<AppContext>(builder =>
        {
            builder.ConfigureAppContext(conn);
        });
    }

    private static void ConfigureAppContext(this DbContextOptionsBuilder builder, string conn)
    {
        builder.UseNpgsql(conn, b => b.EnableRetryOnFailure());
        builder.UseExceptionProcessor();
    }
}