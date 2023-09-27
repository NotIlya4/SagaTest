using Microsoft.EntityFrameworkCore;
using MoneyService.EntityFramework;
using MoneyService.Extensions;

namespace MoneyService;

public class DbConfigurator
{
    private readonly DbType _dbType;
    private readonly IConfiguration _configuration;

    public DbConfigurator(DbType dbType, IConfiguration configuration)
    {
        _dbType = dbType;
        _configuration = configuration;
    }

    public void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
    {
        var _ = _dbType switch
        {
            DbType.Postgres => builder.UseNpgsql(_configuration.GetPostgresConn(), b => b.EnableRetryOnFailure()),
            DbType.Mssql => builder.UseSqlServer(_configuration.GetMssqlConn(), optionsBuilder => optionsBuilder.EnableRetryOnFailure()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum DbType
{
    Postgres,
    Mssql
}