using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using MoneyService.EntityFramework;
using Npgsql;

namespace MoneyService.Extensions;

public static class ServiceCollectionExtensions
{
    public static string GetPostgresConn(this IConfiguration config)
    {
        return config.GetSection("PostgresConn").Get<NpgsqlConnectionStringBuilder>()!.ToString();
    }
    
    public static void AddAppContext(this IServiceCollection services, string conn)
    {
        services.AddDbContextFactory<AppDbContext>(builder =>
        {
            builder.ConfigureAppContext(conn);
        });
        services.AddDbContext<AppDbContext>(builder =>
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