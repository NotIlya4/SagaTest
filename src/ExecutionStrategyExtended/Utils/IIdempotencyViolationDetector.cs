namespace ExecutionStrategyExtended.Utils;

public interface IIdempotencyViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}