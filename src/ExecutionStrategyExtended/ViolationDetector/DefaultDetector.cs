namespace ExecutionStrategyExtended.ViolationDetector;

public class DefaultDetector : IIdempotenceViolationDetector
{
    private readonly IEnumerable<IIdempotenceViolationDetector> _detectors;

    public DefaultDetector(IEnumerable<IIdempotenceViolationDetector> detectors)
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