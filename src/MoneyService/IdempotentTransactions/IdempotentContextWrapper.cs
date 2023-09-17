using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OneOf.Types;
using IsolationLevel = System.Data.IsolationLevel;

namespace MoneyService.IdempotentTransactions;

public class IdempotentDbContextWrapper<TDbContext> where TDbContext : DbContext
{
    private readonly IdempotencyFactory _factory;
    private readonly IIdempotencyViolationDetector _violationDetector;
    private readonly ISerializer _serializer;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public IdempotentDbContextWrapper(IdempotencyFactory factory,
        IIdempotencyViolationDetector violationDetector, ISerializer serializer,
        IDbContextFactory<TDbContext> dbContextFactory)
    {
        _factory = factory;
        _violationDetector = violationDetector;
        _serializer = serializer;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<TResponse> ExecuteIdempotentTransaction<TResponse>(Func<TDbContext, Task<TResponse>> action,
        string idempotencyToken, IsolationLevel isolationLevel)
    {
        
        var token = _factory.CreateIdempotencyToken(idempotencyToken);
        var strategy = context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteInTransactionAsync(
            async () =>
            {
                var context = await _dbContextFactory.CreateDbContextAsync();
                return HandleSingleAttempt(c)
            },
            isolationLevel);
    }

    private async Task<TResponse> HandleSingleAttempt<TResponse>(TDbContext context,
        Func<TDbContext, Task<TResponse>> action,
        IdempotencyToken idempotencyToken)
    {
        var addResult = await TryToAddIdempotencyToken(idempotencyToken);

        addResult.ThrowIfUnknownException();
        if (addResult.Value is AlreadyExists)
        {
            context.DetachIdempotencyToken(idempotencyToken);
            var dbToken = await context.GetIdempotencyToken(idempotencyToken.Id);
            return _serializer.Deserialize<TResponse>(dbToken.Response);
        }

        var response = await action(context);
        idempotencyToken.Response = _serializer.Serialize(response);

        await context.SaveChangesAsync();

        return response;
    }

    public async Task<IdempotencyTokenAddResult> TryToAddIdempotencyToken(IdempotencyToken idempotencyToken)
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
                return new AlreadyExists(e);
            }

            return new UnknownException(e);
        }

        return new Success();
    }
}

public struct AlreadyExists
{
    public Exception Exception { get; }

    public AlreadyExists(Exception exception)
    {
        Exception = exception;
    }
}

public struct UnknownException
{
    public Exception Exception { get; }

    public UnknownException(Exception exception)
    {
        Exception = exception;
    }
}

public interface IClock
{
    DateTime GetCurrentTime();
}

public class Clock : IClock
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}

public interface ISerializer
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string rawObj);
}

public class Serializer : ISerializer
{
    public string Serialize<T>(T obj)
    {
        return JObject.FromObject(obj).ToString();
    }

    public T Deserialize<T>(string rawObj)
    {
        return JObject.Parse(rawObj).ToObject<T>()!;
    }
}

public class IdempotencyFactory
{
    private readonly IClock _clock;
    private readonly ISerializer _serializer;

    public IdempotencyFactory(IClock clock, ISerializer serializer)
    {
        _clock = clock;
        _serializer = serializer;
    }

    public IdempotencyToken CreateIdempotencyToken<T>(string idempotencyToken, T response)
    {
        var time = _clock.GetCurrentTime();
        var rawResponse = _serializer.Serialize(response);

        return new IdempotencyToken(idempotencyToken, rawResponse, time);
    }

    public IdempotencyToken CreateIdempotencyToken(string idempotencyToken)
    {
        var time = _clock.GetCurrentTime();

        return new IdempotencyToken(idempotencyToken, "", time);
    }
}

public interface IIdempotencyViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}

public class IdempotencyViolationDetector : IIdempotencyViolationDetector
{
    public bool IsUniqueConstraintViolation(Exception e)
    {
        return e is UniqueConstraintException;
    }
}