using Microsoft.Extensions.DependencyInjection;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using UnitTests.Fixture;

namespace UnitTests.Tests;

public class MoneyServiceTests : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestFixture _fixture;
    private readonly MoneyService.Services.UserCrud _service;
    private readonly AppDbContext _dbContext;
    private readonly IServiceScope _scope;

    public MoneyServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _dbContext = _scope.ServiceProvider.GetAppContext();
        _service = _scope.ServiceProvider.GetMoneyService();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}