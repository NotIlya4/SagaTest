using ExecutionStrategyExtended.Utils;
using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Models;

public class IdempotencyFactory
{
    private readonly ISystemClock _clock;
    private readonly IResponseSerializer _responseSerializer;

    public IdempotencyFactory(ISystemClock clock, IResponseSerializer responseSerializer)
    {
        _clock = clock;
        _responseSerializer = responseSerializer;
    }

    public IdempotencyToken CreateIdempotencyToken<T>(string idempotencyToken, T response)
    {
        var time = _clock.UtcNow;
        var rawResponse = _responseSerializer.Serialize(response);

        return new IdempotencyToken(idempotencyToken, rawResponse, time);
    }

    public IdempotencyToken CreateIdempotencyToken(string idempotencyToken)
    {
        var time = _clock.UtcNow;

        return new IdempotencyToken(idempotencyToken, "", time);
    }
}