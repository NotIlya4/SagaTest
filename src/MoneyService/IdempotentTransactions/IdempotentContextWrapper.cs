using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;
using OneOf.Types;
using IsolationLevel = System.Data.IsolationLevel;

namespace MoneyService.IdempotentTransactions;

public class IdempotentDbContextWrapper<TDbContext> where TDbContext : DbContext
{
    private readonly IdempotencyFactory _factory;
    private readonly IIdempotencyViolationDetector _violationDetector;
    private readonly ISerializer _serializer;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;
    private readonly DbContextProvidePolicy _dbContextProvidePolicy;

    public IdempotentDbContextWrapper(IdempotencyFactory factory,
        IIdempotencyViolationDetector violationDetector, ISerializer serializer,
        IDbContextFactory<TDbContext> dbContextFactory, DbContextProvidePolicy dbContextProvidePolicy)
    {
        _factory = factory;
        _violationDetector = violationDetector;
        _serializer = serializer;
        _dbContextFactory = dbContextFactory;
        _dbContextProvidePolicy = dbContextProvidePolicy;
    }

    public async Task<TResponse> WithIdempotentTransaction<TResponse>(Func<TDbContext, Task<TResponse>> action, string idempotencyToken, IsolationLevel isolationLevel)
    {
        await using var mainContext = await _dbContextFactory.CreateDbContextAsync();
        var strategy = mainContext.Database.CreateExecutionStrategy();
        var token = _factory.CreateIdempotencyToken(idempotencyToken);
        var dbContextProvider = new DbContextProvider<TDbContext>(mainContext, _dbContextProvidePolicy, _dbContextFactory);
        
        return await strategy.ExecuteAsync(async () =>
        {
            var context = await dbContextProvider.ProvideDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await HandleSingleAttempt(context, action, token);
            await transaction.CommitAsync();

            return response;
        });
    }

    public async Task<TResponse> HandleSingleAttempt<TResponse>(TDbContext context,
        Func<TDbContext, Task<TResponse>> action,
        IdempotencyToken idempotencyToken)
    {
        var addResult = await TryToAddIdempotencyToken(context, idempotencyToken);

        addResult.ThrowIfUnknownException();
        if (addResult.Value is AlreadyExists)
        {
            return await HandleAlreadyAddedToken<TResponse>(context, idempotencyToken);
        }

        var response = await action(context);
        idempotencyToken.Response = _serializer.Serialize(response);

        await context.SaveChangesAsync();

        return response;
    }

    public async Task<IdempotencyTokenAddResult> TryToAddIdempotencyToken(TDbContext context, IdempotencyToken idempotencyToken)
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

            return new UnknownException(e);
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

public class DbContextProvider<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _mainContext;
    private readonly DbContextProvidePolicy _policy;
    private readonly IDbContextFactory<TDbContext> _factory;
    private TDbContext? _factoryContext;

    public DbContextProvider(TDbContext mainContext, DbContextProvidePolicy policy, IDbContextFactory<TDbContext> factory)
    {
        _mainContext = mainContext;
        _policy = policy;
        _factory = factory;
    }

    public async Task<TDbContext> ProvideDbContext()
    {
        return _policy switch
        {
            DbContextProvidePolicy.ProvideSame => ProvideForSame(),
            DbContextProvidePolicy.ClearChangeTracker => ProvideForClear(),
            DbContextProvidePolicy.CreateNew => await ProvideForNew(),
            _ => throw new NotImplementedException()
        };
    }

    private TDbContext ProvideForSame()
    {
        return _mainContext;
    }

    private TDbContext ProvideForClear()
    {
        _mainContext.ChangeTracker.Clear();
        return _mainContext;
    }

    private async Task<TDbContext> ProvideForNew()
    {
        if (_factoryContext is not null)
        {
            await _factoryContext.DisposeAsync();
        }

        _factoryContext = await _factory.CreateDbContextAsync();
        return _factoryContext;
    }
}

public enum DbContextProvidePolicy
{
    ProvideSame,
    ClearChangeTracker,
    CreateNew
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