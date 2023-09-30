namespace ExecutionStrategyExtended.DbContextRetrier;

public static class DbContextRetriers
{
    public static DbContextRetrierConfiguration CreateNewDbContextStrategy(bool disposePreviousContext = false)
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static DbContextRetrierConfiguration ClearChangeTrackerStrategy()
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.ClearChangeTracker
        };
    }
    
    public static DbContextRetrierConfiguration UseSameDbContextStrategy()
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.UseSame
        };
    }
}