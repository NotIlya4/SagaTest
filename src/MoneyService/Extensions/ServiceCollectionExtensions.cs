using Microsoft.EntityFrameworkCore;
using MoneyService.EntityFramework;
using NotIlya.SqlConnectionString.Extensions;
using Npgsql;

namespace MoneyService.Extensions;

public static class ServiceCollectionExtensions
{
    public static string GetPostgresConn(this IConfiguration config)
    {
        return config.GetSection("PostgresConn").Get<NpgsqlConnectionStringBuilder>()!.ToString();
    }
    
    public static string GetMssqlConn(this IConfiguration config)
    {
        return config.GetSqlConnectionString("MssqlConn");
    }
    
    public static void AddAppContext(this IServiceCollection services, string conn)
    {
        services.AddDbContextFactory<AppDbContext>(builder =>
        {
            builder.UseNpgsql(conn, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        });
    }
}