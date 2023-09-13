using EfTest.Models;
using Microsoft.EntityFrameworkCore;

namespace EfTest.EntityFramework;

public class AppContext : DbContext
{
    public DbSet<Idempotency> Idempotencies { get; private set; }
    
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var idempotency = modelBuilder.Entity<Idempotency>();
        idempotency.Property(i => i.Id).HasMaxLength(255);
    }
}