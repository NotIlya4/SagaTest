namespace UnitTests.PostgresBootstrapping.Bootstrapper;

public interface IDbBootstrapper : IDisposable
{
    void Bootstrap();
    void Destroy();
    void Clean();
}