using Microsoft.EntityFrameworkCore;
using MoneyService.IdempotentTransactions;
using MoneyService.Models;

namespace MoneyService.EntityFramework;

public class AppContext : DbContext
{
    public DbSet<IdempotencyToken> Idempotencies { get; private set; } = null!;
    public DbSet<User> Users { get; private set; } = null!;
    
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var idempotency = modelBuilder.Entity<IdempotencyToken>();
        idempotency.Property(i => i.Id).HasMaxLength(255);
    }
}