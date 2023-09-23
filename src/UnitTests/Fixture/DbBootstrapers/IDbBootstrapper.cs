using Microsoft.Extensions.Configuration;

namespace UnitTests.Fixture;

public interface IDbBootstrapper
{
    void Bootstrap();
    void Destroy();
}

public interface IDbCleaner
{
    void Clean();
}

public interface IDbBootstrapperCleaner : IDbBootstrapper, IDbCleaner
{
    
}