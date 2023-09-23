namespace ExecutionStrategyExtended;

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