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
        finally
        {
            DetachToken(idempotencyToken);
        }

        return new IdempotencyTokenSaveResult(false);
    }

    public async Task UpdateAndSaveToken(IdempotencyToken idempotencyToken)
    {
        _context.Update(idempotencyToken);
        try
        {
            await _context.SaveChangesAsync();
        }
        finally
        {
            DetachToken(idempotencyToken);
        }
    }

    public async Task<IdempotencyToken> GetFreshToken(IdempotencyToken idempotencyToken)
    {
        return await _context
            .IdempotencyTokens()
            .AsNoTracking()
            .SingleAsync(x => x.Id == idempotencyToken.Id);
    }

    public void DetachToken(IdempotencyToken idempotencyToken)
    {
        _context.Entry(idempotencyToken).State = EntityState.Detached;
    }
}