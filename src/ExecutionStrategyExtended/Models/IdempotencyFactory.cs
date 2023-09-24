using Microsoft.Extensions.Internal;

namespace ExecutionStrategyExtended.Models;

public class IdempotencyFactory
{
    private readonly ISystemClock _clock;
    private readonly ISerializer _serializer;

    public IdempotencyFactory(ISystemClock clock, ISerializer serializer)
    {
        _clock = clock;
        _serializer = serializer;
    }

    public IdempotencyToken CreateIdempotencyToken<T>(string idempotencyToken, T response)
    {
        var time = _clock.UtcNow;
        var rawResponse = _serializer.Serialize(response);

        return new IdempotencyToken(idempotencyToken, rawResponse, time);
    }

    public IdempotencyToken CreateIdempotencyToken(string idempotencyToken)
    {
        var time = _clock.UtcNow;

        return new IdempotencyToken(idempotencyToken, "", time);
    }
}