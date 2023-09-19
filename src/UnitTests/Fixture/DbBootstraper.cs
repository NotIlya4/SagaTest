using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;
using MoneyService.Extensions;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace UnitTests.Fixture;

public class SoftDbContextBootstraper : IDbBootstraper
{
    private readonly AppContext _context;

    public SoftDbContextBootstraper(AppContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        Clear();
    }

    public void PrepareReadyEmptyDb()
    {
        Clear();
    }

    public void Clear()
    {
        _context.RemoveRange(_context.Idempotencies.ToList());
        _context.RemoveRange(_context.Users.ToList());
        _context.SaveChanges();
    }
}

public class DbContextBootstraper : IDbBootstraper
{
    private readonly AppContext _context;

    public DbContextBootstraper(AppContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        Clear();
    }

    public void PrepareReadyEmptyDb()
    {
        Clear();
    }

    public void Clear()
    {
        _context.ReloadDb();
    }
}

public class FluentDockerDbBootstraper : IDbBootstraper
{
    private readonly PostgresContainerOptions _postgresOptions;
    private IService? _container;

    public FluentDockerDbBootstraper(PostgresContainerOptions postgresOptions)
    {
        _postgresOptions = postgresOptions;
    }
    
    public void Dispose()
    {
        Clear();
    }

    public void PrepareReadyEmptyDb()
    {
        if (_container is not null)
        {
            _container.Dispose();
        }
        
        _container = new Builder()
            .UseContainer().UsePostgresContainer()
            .Build().Start();
    }

    public void Clear()
    {
        _container?.Dispose();
    }
}

public interface IDbBootstraper : IDisposable
{
    void PrepareReadyEmptyDb();
    void Clear();
}