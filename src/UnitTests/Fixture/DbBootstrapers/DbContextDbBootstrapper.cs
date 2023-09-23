using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;
using MoneyService.Extensions;

namespace UnitTests.Fixture;

public class HardDbContextBootstrapper : IDbBootstrapperCleaner 
{
    private readonly AppDbContext _context;

    public HardDbContextBootstrapper(AppDbContext context)
    {
        _context = context;
    }
    
    public void Bootstrap()
    {
        Clean();
    }

    public void Destroy()
    {
        Clean();
    }
    
    public void Clean()
    {
        _context.ReloadDb();
    }
}

public class SoftDbContextBootstrapper : IDbBootstrapperCleaner
{
    private readonly AppDbContext _context;

    public SoftDbContextBootstrapper(AppDbContext context)
    {
        _context = context;
    }
    
    public void Bootstrap()
    {
        Clean();
    }

    public void Destroy()
    {
        Clean();
    }
    
    public void Clean()
    {
        _context.RemoveRange(_context.Idempotencies.ToList());
        _context.RemoveRange(_context.Users.ToList());
        _context.SaveChanges();
    }
}