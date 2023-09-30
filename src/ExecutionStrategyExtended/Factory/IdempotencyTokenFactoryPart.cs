using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Configuration;

internal class IdempotencyTokenFactoryPart
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;
    private readonly IServiceProvider _provider;

    public IdempotencyTokenFactoryPart(IExecutionStrategyInternalConfiguration configuration, IServiceProvider provider)
    {
        _configuration = configuration;
        _provider = provider;
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