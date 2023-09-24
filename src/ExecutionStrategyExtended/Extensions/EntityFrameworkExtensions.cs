﻿using System.Data;
using ExecutionStrategyExtended.ExecutionStrategy;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Extensions;

public static class EntityFrameworkExtensions
{
    internal static async Task<TResponse> ExecuteInTransactionAsync<TDbContext, TResponse>(
        this TrueExecutionStrategy<TDbContext> trueStrategy, Func<TDbContext, Task<TResponse>> action, 
        IsolationLevel isolationLevel) where TDbContext : DbContext
    {
        return await trueStrategy.ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await action(context);

            await transaction.CommitAsync();

            return response;
        });
    }
    
    internal static DbSet<IdempotencyToken> IdempotencyTokens(this DbContext context)
    {
        return context.Set<IdempotencyToken>();
    }

    internal static void Detach<T>(this DbContext context, T entity)
    {
        context.Entry(entity!).State = EntityState.Detached;
    }
    
    public static void AddIdempotencyTokens(this ModelBuilder builder, IdempotencyTokenTableOptions? options = null)
    {
        options ??= new IdempotencyTokenTableOptions();
        
        builder.Entity<IdempotencyToken>(typeBuilder =>
        {
            typeBuilder.Property(x => x.Id).HasMaxLength(options.MaxLength);
            typeBuilder.ToTable(options.TableName);
        });
    }
}

public record IdempotencyTokenTableOptions
{
    public int MaxLength { get; set; } = 255;
    public string TableName { get; set; } = "IdempotencyTokens";
}