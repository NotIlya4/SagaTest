﻿using EntityFramework.Exceptions.Common;

namespace ExecutionStrategyExtended;

public interface IIdempotencyViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}

public class IdempotencyViolationDetector : IIdempotencyViolationDetector
{
    public bool IsUniqueConstraintViolation(Exception e)
    {
        return e is UniqueConstraintException;
    }
}