using EfTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyService;
using MoneyService.Models;
using AppContext = MoneyService.EntityFramework.AppContext;

namespace UnitTests;

public class MoneyServiceTests : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestFixture _fixture;
    private readonly MoneyService.Services.MoneyService _service;
    private readonly AppContext _context;
    private readonly IServiceScope _scope;

    public MoneyServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _context = _scope.ServiceProvider.GetAppContext();
        _service = _scope.ServiceProvider.GetMoneyService();
    }

    [Fact]
    public async Task InsertUser_RegularUser_UserPersistInDb()
    {
        var expect = new User(0, 999, false);
        await _service.InsertUser(expect, "a");
        _context.ChangeTracker.Clear();

        var result = await _context.Users.ToListAsync();
        
        Assert.Equal(expect, result.Single());
    }

    [Fact]
    public async Task Test()
    {
        var concurrentScope = _fixture.CreateScope();
        var concurrentDbContext = concurrentScope.ServiceProvider.GetAppContext();
        try
        {
            await _context.BeginIdempotentTransaction("a");
            await concurrentDbContext.BeginIdempotentTransaction("a");
        }
        finally
        {
            await _context.RollbackTransaction();
            await concurrentDbContext.RollbackTransaction();
            concurrentScope.Dispose();
        }
    }

    [Fact]
    public async Task Test2()
    {
        ResilientTransaction.
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}