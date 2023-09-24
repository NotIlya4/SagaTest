using EntityFramework.Exceptions.Common;

namespace ExecutionStrategyExtended.Utils;

public interface IIdempotencyViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}