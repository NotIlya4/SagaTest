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
        services.AddDbContext<AppContext>(builder =>
        {
            builder.UseNpgsql(conn, b => b.EnableRetryOnFailure());
            builder.UseExceptionProcessor();
        });
    }
}