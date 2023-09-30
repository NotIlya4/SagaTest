using ExecutionStrategyExtended.StrategyExtended;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.IdempotenceToken;

internal class IdempotencyTokenManager
{
    private readonly ISystemClock _clock;
    private readonly IResponseSerializer _responseSerializer;

    public IdempotencyTokenManager(ISystemClock clock, IResponseSerializer responseSerializer)
    {
        _clock = clock;
        _responseSerializer = responseSerializer;
    }

    public IdempotencyToken CreateIdempotencyToken<T>(string idempotencyToken, T response)
    {
        var time = _clock.UtcNow.UtcDateTime;
        var rawResponse = _responseSerializer.Serialize(response);

        return new IdempotencyToken(idempotencyToken, rawResponse, time);
    }

    public IdempotencyToken CreateIdempotencyToken(string idempotencyToken)
    {
        var time = _clock.UtcNow.UtcDateTime;

        return new IdempotencyToken(idempotencyToken, "", time);
    }

    public void SetResponse<TResponse>(IdempotencyToken token, TResponse response)
    {
        token.Response = _responseSerializer.Serialize(response);
    }

    public TResponse GetResponse<TResponse>(IdempotencyToken token)
    {
        return _responseSerializer.Deserialize<TResponse>(token.Response);
    }
}