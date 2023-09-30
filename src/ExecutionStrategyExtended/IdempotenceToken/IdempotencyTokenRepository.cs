using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.IdempotenceToken;

internal class IdempotencyTokenRepository
{
    private readonly DbContext _context;
    private readonly IIdempotenceViolationDetector _violationDetector;

    public IdempotencyTokenRepository(DbContext context, IIdempotenceViolationDetector violationDetector)
    {
        _context = context;
        _violationDetector = violationDetector;
    }

    public async Task<IdempotencyTokenSaveResult> AddAndSaveToken(IdempotencyToken idempotencyToken)
    {
        return await WithDetachToken(async () =>
        {
            _context.IdempotencyTokens().Add(idempotencyToken);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (_violationDetector.IsUniqueConstraintViolation(e))
                {
                    return new IdempotencyTokenSaveResult(true);
                }

                throw;
            }

            return new IdempotencyTokenSaveResult(false);
        }, idempotencyToken);
    }

    public async Task UpdateAndSaveToken(IdempotencyToken idempotencyToken)
    {
        await WithDetachToken(async () =>
        {
            _context.Update(idempotencyToken);
            await _context.SaveChangesAsync();
            return true;
        }, idempotencyToken);
    }

    public async Task<IdempotencyToken> GetFreshToken(IdempotencyToken idempotencyToken)
    {
        return await _context
            .IdempotencyTokens()
            .AsNoTracking()
            .SingleAsync(x => x.Id == idempotencyToken.Id);
    }

    private async Task<TResult> WithDetachToken<TResult>(Func<Task<TResult>> action, IdempotencyToken token)
    {
        try
        {
            return await action();
        }
        finally
        {
            _context.Entry(token).State = EntityState.Detached;
        }
    }
}