using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace UnitTests.Fixture.PostgresBootstrapper;

public class PostgresContainer : IDisposable
{
    private readonly PostgresContainerOptions _options;
    private IService? _container;
    public bool IsDisposed { get; private set; }

    public PostgresContainer(PostgresContainerOptions options)
    {
        _options = options;
    }

    public void Bootstrap()
    {
        lock (this)
        {
            CheckDisposed();
            
            Destroy();
        
            _container = new Builder()
                .UsePostgresContainer(_options)
                .Build().Start();
        }
    }

    public void Destroy()
    {
        lock (this)
        {
            CheckDisposed();
            
            _container?.Dispose();
            _container = null;
        }
    }

    private void CheckDisposed()
    {
        if (IsDisposed)
        {
            throw new InvalidOperationException("Container disposed");
        }
    }

    public void Dispose()
    {
        lock (this)
        {
            Destroy();
            IsDisposed = true;
        }
    }
}