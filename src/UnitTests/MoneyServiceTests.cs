using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyService.Extensions;
using MoneyService.Models;
using UnitTests.Fixture;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace UnitTests;

public class MoneyServiceTests : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestFixture _fixture;
    private readonly MoneyService.Services.UserCrud _service;
    private readonly AppContext _context;
    private readonly IServiceScope _scope;

    public MoneyServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _context = _scope.ServiceProvider.GetAppContext();
        _service = _scope.ServiceProvider.GetMoneyService();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}