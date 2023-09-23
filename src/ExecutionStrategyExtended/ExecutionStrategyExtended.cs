using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;
using OneOf.Types;
using IsolationLevel = System.Data.IsolationLevel;

namespace ExecutionStrategyExtended;

public class ExecutionStrategyExtended<TDbContext> where TDbContext : DbContext
{
    private readonly IdempotencyFactory _factory;
    private readonly IIdempotencyViolationDetector _violationDetector;
    private readonly ISerializer _serializer;
    private readonly TrueExecutionStrategyFactory<TDbContext> _executionStrategyFactory;

    public ExecutionStrategyExtended(IdempotencyFactory factory,
        IIdempotencyViolationDetector violationDetector, ISerializer serializer,
        TrueExecutionStrategyFactory<TDbContext> executionStrategyFactory)
    {
        _factory = factory;
        _violationDetector = violationDetector;
        _serializer = serializer;
        _executionStrategyFactory = executionStrategyFactory;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        var trueStrategy = _executionStrategyFactory.Create(mainContext);
        return await trueStrategy.ExecuteAsync(action);
    }

    public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel)
    {
        var trueStrategy = _executionStrategyFactory.Create(mainContext);
        return await trueStrategy.ExecuteInTransactionAsync(action, isolationLevel);
    }

    public async Task<TResponse> ExecuteInIdempotentTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action, 
        TDbContext mainContext, string idempotencyToken, IsolationLevel isolationLevel)
    {
        var token = _factory.CreateIdempotencyToken(idempotencyToken);

        return await ExecuteInTransactionAsync(async context => await HandleSingleAttempt(context, action, token),
            mainContext, isolationLevel);
    }

    public async Task<TResponse> HandleSingleAttempt<TResponse>(TDbContext context,
        Func<TDbContext, Task<TResponse>> action,
        IdempotencyToken idempotencyToken)
    {
        var addResult = await AddIdempotencyToken(context, idempotencyToken);

        if (addResult.IsAlreadyExists()) 
            return await HandleAlreadyAddedToken<TResponse>(context, idempotencyToken);

        var response = await action(context);
        idempotencyToken.Response = _serializer.Serialize(response);

        await context.SaveChangesAsync();

        return response;
    }

    public async Task<IdempotencyTokenAddResult> AddIdempotencyToken(TDbContext context, IdempotencyToken idempotencyToken)
    {
        context.IdempotencyTokens().Add(idempotencyToken);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (_violationDetector.IsUniqueConstraintViolation(e))
            {
                return new AlreadyExists(e);
            }

            throw;
        }
        finally
        {
            context.Entry(idempotencyToken).State = EntityState.Unchanged;
        }

        return new Success();
    }

    public async Task<TResponse> HandleAlreadyAddedToken<TResponse>(TDbContext context, IdempotencyToken idempotencyToken)
    {
        context.Detach(idempotencyToken);
        var dbToken = await context
            .IdempotencyTokens()
            .AsNoTracking()
            .SingleAsync(x => x.Id == idempotencyToken.Id);
        return _serializer.Deserialize<TResponse>(dbToken.Response);
    }
}