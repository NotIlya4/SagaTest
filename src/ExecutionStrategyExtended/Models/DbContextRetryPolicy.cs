namespace ExecutionStrategyExtended.Models;

public enum DbContextRetryPolicy
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}