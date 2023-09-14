using System.Data;
using MoneyService.Models;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace MoneyService.Services;

public class UserCrud
{
    private readonly AppContext _context;

    public UserCrud(AppContext context)
    {
        _context = context;
    }

    public async Task InsertUser(User user, Idempotency idempotency)
    {
        var transaction = await _context.BeginIdempotentTransaction(idempotency, IsolationLevel.RepeatableRead);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}