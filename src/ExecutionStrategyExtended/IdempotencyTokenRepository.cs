using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Models;
using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

internal class IdempotencyTokenRepository<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly IIdempotenceViolationDetector _violationDetector;
    private readonly IResponseSerializer _responseSerializer;

    public IdempotencyTokenRepository(TDbContext context, IIdempotenceViolationDetector violationDetector, IResponseSerializer responseSerializer)
    {
        _context = context;
        _violationDetector = violationDetector;
        _responseSerializer = responseSerializer;
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
            _context.Entry(idempotencyToken).State = EntityState.Unchanged;
        }

        return new IdempotencyTokenSaveResult(false);
    }

    public async Task<IdempotencyToken> GetFreshToken(IdempotencyToken idempotencyToken)
    {
        _context.Detach(idempotencyToken);
        return await _context
            .IdempotencyTokens()
            .AsNoTracking()
            .SingleAsync(x => x.Id == idempotencyToken.Id);
    }

    public async Task<TResponse> HandleAction<TResponse>(Func<TDbContext, Task<TResponse>> action, 
        IdempotencyToken idempotencyToken)
    {
        var addResult = await AddAndSaveToken(idempotencyToken);

        if (addResult.IsAlreadyExists)
        {
            var freshTokenFromDb = await GetFreshToken(idempotencyToken);
            return _responseSerializer.Deserialize<TResponse>(freshTokenFromDb.Response);
        }

        var response = await action(_context);
        idempotencyToken.Response = _responseSerializer.Serialize(response);

        await _context.SaveChangesAsync();

        return response;
    }
}