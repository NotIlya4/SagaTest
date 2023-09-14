using Microsoft.EntityFrameworkCore;
using Npgsql;
using AppContext = EfTest.EntityFramework.AppContext;

public static class EfCoreExtensions
{
    public static string GetPostgresConn(this IConfiguration config)
    {
        return config.GetSection("PostgresConn").Get<NpgsqlConnectionStringBuilder>()!.ToString();
    }
    
    public static void AddAppContext(this IServiceCollection services, string conn)
    {
        services.AddDbContext<AppContext>(builder =>
        {
            builder.UseNpgsql(conn);
        });
    }
}