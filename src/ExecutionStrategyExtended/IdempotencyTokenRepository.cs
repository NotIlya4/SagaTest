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
    private readonly IdempotencyTokenManager _tokenManager;

    public IdempotencyTokenRepository(TDbContext context, IIdempotenceViolationDetector violationDetector, IdempotencyTokenManager tokenManager)
    {
        _context = context;
        _violationDetector = violationDetector;
        _tokenManager = tokenManager;
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
            _context.Detach(idempotencyToken);
        }
    }

    public void DetachRawToken(string idempotencyToken)
    {
        var tokenEntry = _context.ChangeTracker
            .Entries<IdempotencyToken>()
            .FirstOrDefault(x => x.Entity.Id == idempotencyToken);

        if (tokenEntry is not null)
            tokenEntry.State = EntityState.Detached;
    }

    public void DetachToken(IdempotencyToken idempotencyToken)
    {
        _context.Entry(idempotencyToken).State = EntityState.Detached;
    }

    public async Task<IdempotencyToken> GetFreshToken(IdempotencyToken idempotencyToken)
    {
        return await _context
            .IdempotencyTokens()
            .AsNoTracking()
            .SingleAsync(x => x.Id == idempotencyToken.Id);
    }
}

internal class IdempotencyTokenService<TDbContext> where TDbContext : DbContext
{
    private readonly IdempotencyTokenRepository<TDbContext> _repository;

    public IdempotencyTokenService(IdempotencyTokenRepository<TDbContext> repository)
    {
        _repository = repository;
    }
    
    public async Task<TResponse> HandleAction<TResponse>(Func<TDbContext, Task<TResponse>> action, 
        IdempotencyToken idempotencyToken)
    {
        var addResult = await AddAndSaveToken(idempotencyToken);

        if (addResult.IsAlreadyExists)
        {
            var freshTokenFromDb = await GetFreshToken(idempotencyToken);
            return _tokenManager.GetResponse<TResponse>(freshTokenFromDb);
        }

        var response = await action(_context);
        _tokenManager.SetResponse(idempotencyToken, response);

        await UpdateAndSaveToken(idempotencyToken);

        return response;
    }
}