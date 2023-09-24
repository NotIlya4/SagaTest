namespace ExecutionStrategyExtended.ViolationDetector;

public interface IIdempotencyViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}