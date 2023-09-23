using System.Data;
using Microsoft.EntityFrameworkCore;
using MoneyService.EntityFramework;
using MoneyService.IdempotentTransactions;
using MoneyService.Models;
using MoneyService.Services;

namespace MoneyService.Extensions;

public static class Extensions
{
    public static async Task WithTransaction(
        this AppDbContext dbContext, Func<Task> action, IsolationLevel isolationLevel)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(isolationLevel);
            await action();
            await transaction.CommitAsync();
        });
    }
    
    public static async Task WithTransaction(
        this AppDbContext dbContext, Func<Task> action)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
    
    public static async Task WithIdempotentTransaction(
        this AppDbContext dbContext, Func<Task> action, IdempotencyToken idempotencyToken, IsolationLevel isolationLevel)
    {
        await dbContext.WithTransaction(async () =>
        {
            dbContext.Idempotencies.Add(idempotencyToken);
            await dbContext.SaveChangesAsync();
            await action();
        }, isolationLevel);
    }
    
    public static async Task WithIdempotentTransaction(
        this AppDbContext dbContext, Func<Task> action, IdempotencyToken idempotencyToken)
    {
        await dbContext.WithTransaction(async () =>
        {
            dbContext.Idempotencies.Add(idempotencyToken);
            await dbContext.SaveChangesAsync();
            await action();
        });
    }

    public static void EnsureDeletedCreated(this DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static void CleanTables(this AppDbContext context)
    {
        context.RemoveRange(context.Idempotencies.ToList());
        context.RemoveRange(context.Users.ToList());
        context.SaveChanges();
    }

    public static AppDbContext GetAppContext(this IServiceProvider services)
    {
        return services.GetRequiredService<AppDbContext>();
    }
    
    public static UserCrud GetMoneyService(this IServiceProvider services)
    {
        return services.GetRequiredService<UserCrud>();
    }
}