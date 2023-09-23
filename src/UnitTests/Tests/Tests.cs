using Microsoft.Extensions.DependencyInjection;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using MoneyService.Models;
using UnitTests.Fixture;

namespace UnitTests.Tests;

public class Tests : IDisposable, IClassFixture<TestFixture>
{
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _services;
    private readonly TestFixture _fixture;
    private readonly AppDbContext _context;

    public Tests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _services = _scope.ServiceProvider;
        _context = _services.GetAppContext();
        _fixture.Bootstrapper.Clean();
    }

    [Fact]
    public void Test()
    {
        _context.Users.Add(new User(0, 123, false));
        _context.SaveChanges();
    }
    
    [Fact]
    public void Test2()
    {
        
    }
    
    public void Dispose()
    {
        _scope.Dispose();
    }
}