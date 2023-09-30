namespace ExecutionStrategyExtended.DbContextRetrier;

public enum DbContextRetrierType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}