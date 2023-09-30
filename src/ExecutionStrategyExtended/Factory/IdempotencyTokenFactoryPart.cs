using ExecutionStrategyExtended.Configuration.Interfaces;
using ExecutionStrategyExtended.IdempotenceToken;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Factory;

internal class IdempotencyTokenFactoryPart
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;

    public IdempotencyTokenFactoryPart(IExecutionStrategyInternalConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IdempotencyTokenManager CreateManager()
    {
        return new IdempotencyTokenManager(_configuration.SystemClock, _configuration.ResponseSerializer);
    }

    public IdempotencyTokenRepository CreateRepository(DbContext context)
    {
        return new IdempotencyTokenRepository(context, _configuration.IdempotenceViolationDetector);
    }

    public IdempotencyTokenService CreateService(DbContext context)
    {
        return new IdempotencyTokenService(CreateRepository(context), CreateManager());
    }
}