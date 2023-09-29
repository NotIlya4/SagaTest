using ExecutionStrategyExtended.Utils;
using ExecutionStrategyExtended.ViolationDetector;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

internal class IdempotencyTokenRepositoryFactory
{
    private readonly IIdempotenceViolationDetector _violationDetector;
    private readonly IResponseSerializer _responseSerializer;

    public IdempotencyTokenRepositoryFactory(IIdempotenceViolationDetector violationDetector, IResponseSerializer responseSerializer)
    {
        _violationDetector = violationDetector;
        _responseSerializer = responseSerializer;
    }

    public IdempotencyTokenRepository<TDbContext> Create<TDbContext>(TDbContext context) where TDbContext : DbContext
    {
        return new IdempotencyTokenRepository<TDbContext>(context, _violationDetector, _responseSerializer);
    }
}