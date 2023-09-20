using Microsoft.Extensions.Configuration;

namespace UnitTests.Fixture;

public interface IDbBootstrapper
{
    void PrepareReadyEmptyDb();
    void Clear();
}