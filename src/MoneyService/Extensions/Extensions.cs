using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MoneyService.Models;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace MoneyService;

public static class Extensions
{
    

    public static async Task<IDbContextTransaction> BeginIdempotentTransaction(
        this AppContext context, Idempotency idempotency, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var transaction = await context.Database.BeginTransactionAsync(isolationLevel);
        context.Idempotencies.Add(idempotency);
        await context.SaveChangesAsync();
        return transaction;
    }

    public static async Task CommitTransaction(this DbContext context)
    {
        await context.Database.CommitTransactionAsync();
    }

    public static async Task RollbackTransaction(this DbContext context)
    {
        await context.Database.RollbackTransactionAsync();
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
    
    public static Services.MoneyService GetMoneyService(this IServiceProvider services)
    {
        return services.GetRequiredService<Services.MoneyService>();
    }
}