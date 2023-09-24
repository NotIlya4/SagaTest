using MoneyService.EntityFramework;
using MoneyService.Services;

namespace MoneyService.Extensions;

public static class Extensions
{
    public static void CleanTables(this AppDbContext context)
    {
        context.RemoveRange(context.Idempotencies.ToList());
        context.RemoveRange(context.Users.ToList());
        context.SaveChanges();
    }
    
    public static void EnsureDeletedCreated(this AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
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