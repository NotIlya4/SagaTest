namespace UnitTests.PostgresBootstrapping.Bootstrapper;

public class DelegateDbBootstrapper : IDbBootstrapper
{
    public Action? BootstrapAction { get; set; }
    public Action? DestroyAction { get; set; }
    public Action? CleanAction { get; set; }
    public Action? DisposeAction { get; set; }
    
    public void Bootstrap()
    {
        if (BootstrapAction is not null)
        {
            BootstrapAction();
        }
    }

    public void Destroy()
    {
        if (DestroyAction is not null)
        {
            DestroyAction();
        }
    }

    public void Clean()
    {
        if (CleanAction is not null)
        {
            CleanAction();
        }
    }

    public void Dispose()
    {
        if (DisposeAction is not null)
        {
            DisposeAction();
        }
        else
        {
            Destroy();
        }
    }
}