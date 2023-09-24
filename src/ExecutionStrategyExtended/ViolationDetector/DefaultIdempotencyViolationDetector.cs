namespace ExecutionStrategyExtended.ViolationDetector;

public class DefaultIdempotencyViolationDetector : IIdempotencyViolationDetector
{
    private readonly IEnumerable<IIdempotencyViolationDetector> _detectors;

    public DefaultIdempotencyViolationDetector(IEnumerable<IIdempotencyViolationDetector> detectors)
    {
        _detectors = detectors;
    }

    public bool IsUniqueConstraintViolation(Exception e)
    {
        foreach (var detector in _detectors)
        {
            if (detector.IsUniqueConstraintViolation(e))
            {
                return true;
            }
        }

        return false;
    }
}