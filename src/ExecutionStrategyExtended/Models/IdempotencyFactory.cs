namespace ExecutionStrategyExtended.Models;

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