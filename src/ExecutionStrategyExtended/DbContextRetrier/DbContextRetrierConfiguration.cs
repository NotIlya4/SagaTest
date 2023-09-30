namespace ExecutionStrategyExtended.DbContextRetrier;

public class DbContextRetrierConfiguration
{
    public DbContextRetrierType DbContextRetrierType { get; set; }
    public bool DisposePreviousContext { get; set; }
}