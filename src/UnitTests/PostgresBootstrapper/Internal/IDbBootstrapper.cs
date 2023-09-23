namespace UnitTests.Fixture.PostgresBootstrapper;

public interface IDbBootstrapper : IDisposable
{
    void Bootstrap();
    void Destroy();
    void Clean();
}