namespace ExecutionStrategyExtended.ViolationDetector;

public interface IIdempotenceViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}