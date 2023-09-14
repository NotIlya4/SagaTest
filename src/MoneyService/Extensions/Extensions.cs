using System.Data;
using Microsoft.EntityFrameworkCore;
using MoneyService.Models;
using MoneyService.Services;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace MoneyService.Extensions;

public static class Extensions
{
    public static async Task WithTransaction(
        this AppContext context, Func<Task> action, IsolationLevel isolationLevel)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);
            await action();
            await transaction.CommitAsync();
        });
    }
    
    public static async Task WithTransaction(
        this AppContext context, Func<Task> action)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
    
    public static async Task WithIdempotentTransaction(
        this AppContext context, Func<Task> action, Idempotency idempotency, IsolationLevel isolationLevel)
    {
        await context.WithTransaction(async () =>
        {
            context.Idempotencies.Add(idempotency);
            await context.SaveChangesAsync();
            await action();
        }, isolationLevel);
    }
    
    public static async Task WithIdempotentTransaction(
        this AppContext context, Func<Task> action, Idempotency idempotency)
    {
        await context.WithTransaction(async () =>
        {
            context.Idempotencies.Add(idempotency);
            await context.SaveChangesAsync();
            await action();
        });
    }

    public static async Task ReloadDbAsync(this DbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
    
    public static void ReloadDb(this DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static AppContext GetAppContext(this IServiceProvider services)
    {
        return services.GetRequiredService<AppContext>();
    }
    
    public static UserCrud GetMoneyService(this IServiceProvider services)
    {
        return services.GetRequiredService<UserCrud>();
    }
}