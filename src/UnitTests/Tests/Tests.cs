using ExecutionStrategyExtended;
using ExecutionStrategyExtended.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using MoneyService.Models;
using UnitTests.Fixture;

namespace UnitTests.Tests;

public class Tests : IDisposable, IClassFixture<TestFixture>
{
    private readonly IServiceScope _scope;
    private readonly IExecutionStrategyExtended<AppDbContext> _strategy;
    private readonly IServiceProvider _services;
    private readonly TestFixture _fixture;
    private readonly AppDbContext _context;

    public Tests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _services = _scope.ServiceProvider;
        _strategy = _services.GetRequiredService<IExecutionStrategyExtended<AppDbContext>>();
        _context = _services.GetAppContext();
        _fixture.Bootstrapper.Clean();
    }

    [Fact]
    public async Task Test()
    {
        bool isThrown = false;
        await _strategy.ExecuteAsync(async context =>
        {
            var user = new User(0, 123, false);

            context.Users.Add(user);

            if (!isThrown)
            {
                throw new TimeoutException();
            }

            await context.SaveChangesAsync();
            
            return new object();
        }, _context);
        
        _context.ChangeTracker.Clear();
        var users = await _context.Users.ToListAsync();
        
        Assert.Single(users);
    }
    
    public void Dispose()
    {
        _scope.Dispose();
    }
}