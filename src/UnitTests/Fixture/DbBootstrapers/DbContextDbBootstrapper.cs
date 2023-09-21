using Microsoft.Extensions.Configuration;
using MoneyService.EntityFramework;
using MoneyService.Extensions;

namespace UnitTests.Fixture;

public class DbContextDbBootstrapper : IDbBootstrapper
{
    private readonly AppDbContext _context;
    private readonly bool _isSoft;

    public DbContextDbBootstrapper(AppDbContext context, bool isSoft)
    {
        _context = context;
        _isSoft = isSoft;
    }

    public void Bootstrap()
    {
        Clear();
    }

    public void Clear()
    {
        if (_isSoft)
        {
            SoftClear();
        }
        else
        {
            HardClear();
        }
    }

    private void SoftClear()
    {
        _context.RemoveRange(_context.Idempotencies.ToList());
        _context.RemoveRange(_context.Users.ToList());
        _context.SaveChanges();
    }

    private void HardClear()
    {
        _context.ReloadDb();
    }
}