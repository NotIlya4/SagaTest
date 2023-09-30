using ExecutionStrategyExtended.IdempotenceToken;
using MoneyService.EntityFramework;
using MoneyService.Models;

namespace MoneyService.Services;

public class UserCrud
{
    private readonly AppDbContext _dbContext;

    public UserCrud(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertUser(User user, IdempotencyToken idempotencyToken)
    {
    }
}