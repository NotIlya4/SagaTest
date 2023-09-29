using ExecutionStrategyExtended.Extensions;
using Microsoft.EntityFrameworkCore;
using MoneyService.Models;

namespace MoneyService.EntityFramework;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; private set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddIdempotencyTokens();
    }
}