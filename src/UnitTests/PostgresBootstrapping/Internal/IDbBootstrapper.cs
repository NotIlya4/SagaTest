namespace UnitTests.PostgresBootstrapping.Internal;

public interface IDbBootstrapper : IDisposable
{
    void Bootstrap();
    void Destroy();
    void Clean();
}