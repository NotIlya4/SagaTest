using ExecutionStrategyExtended.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ExecutionStrategyExtended.ViolationDetector;

public class DefaultPostgresIdempotencyViolationDetector : IIdempotencyViolationDetector
{
    private readonly IdempotencyTokenTableOptions _tableOptions;

    public DefaultPostgresIdempotencyViolationDetector(IdempotencyTokenTableOptions tableOptions)
    {
        _tableOptions = tableOptions;
    }
    
    public bool IsUniqueConstraintViolation(Exception e)
    {
        if (e is DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException is PostgresException postgresException)
            {
                return IsUniqueConstraintViolationInternal(postgresException);
            }
        }

        return false;
    }

    private bool IsUniqueConstraintViolationInternal(PostgresException postgresException)
    {
        return postgresException.ConstraintName == _tableOptions.PrimaryKeyConstraintName
               && postgresException.TableName == _tableOptions.TableName
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation;
    }
}