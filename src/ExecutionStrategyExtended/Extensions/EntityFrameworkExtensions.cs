using ExecutionStrategyExtended.Configuration;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Extensions;

public static class EntityFrameworkExtensions
{
    internal static DbSet<IdempotencyToken> IdempotencyTokens(this DbContext context)
    {
        return context.Set<IdempotencyToken>();
    }
    
    public static void AddIdempotencyTokens(this ModelBuilder builder, IdempotencyTokenTableConfiguration? options = null)
    {
        options ??= new IdempotencyTokenTableConfiguration();
        
        builder.Entity<IdempotencyToken>(typeBuilder =>
        {
            typeBuilder.HasKey(x => x.Id).HasName(options.PrimaryKeyConstraintName);
            typeBuilder.Property(x => x.Id).HasMaxLength(options.MaxLength);
            typeBuilder.ToTable(options.TableName);
        });
    }
}