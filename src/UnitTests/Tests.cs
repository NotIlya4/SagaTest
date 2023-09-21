using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using MoneyService.IdempotentTransactions;
using MoneyService.Models;
using UnitTests.Fixture;

namespace UnitTests;

public class Tests : IDisposable, IClassFixture<TestFixture>
{
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _services;
    private readonly TestFixture _fixture;
    private readonly AppDbContext _dbContext;
    private readonly IdempotentDbContextWrapper<AppDbContext> _wrapper;

    public Tests(TestFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.CreateScope();
        _services = _scope.ServiceProvider;
        _dbContext = _services.GetAppContext();
        _wrapper = _services.GetRequiredService<IdempotentDbContextWrapper<AppDbContext>>();
        _fixture.DbBootstrapper.Bootstrap();
    }
    
    

    // [Fact]
    // public async Task Test()
    // {
    //     var user1 = new User(0, 999, false);
    //
    //     _context.Add(user1);
    //     await _context.SaveChangesAsync();
    //     _context.Detach(user1);
    //
    //     await _wrapper.AutoRetryTransaction(
    //         async (context) =>
    //         {
    //             context.Attach(user1);
    //             user1.Money = 123;
    //
    //             await context.SaveChangesAsync();
    //
    //             return user1;
    //         },
    //         "a",
    //         IsolationLevel.ReadCommitted);
    // }
    
    // [Fact]
    // public async Task Test()
    // {
    //     var strategy = _context.Database.CreateExecutionStrategy();
    //     await ExecutionStrategyExtensions.ExecuteInTransactionAsync(
    //         strategy,
    //         new EmptyState(),
    //         async (_, _) =>
    //         {
    //             var idempotencyToken = new IdempotencyToken(token);
    //             _context.Add(idempotencyToken);
    //
    //             try
    //             {
    //                 await _context.SaveChangesAsync();
    //             }
    //             catch (DbUpdateException exception)
    //             {
    //                 // idempotency token staff
    //             }
    //             
    //             // rest of code
    //         },
    //         async (_, _) => false,
    //         async (c, _) => await c.Database.BeginTransactionAsync(isolationLevel));
    // }
    //
    // public class State { }
    
    // [Fact]
    // public async Task Test()
    // {
    //     var response = await _wrapper.ExecuteIdempotentTransaction(
    //         async (context) =>
    //         {
    //             var user = new User(0, 999, false);
    //             context.Users.Add(user);
    //             await context.SaveChangesAsync();
    //             return user;
    //         },
    //         "a",
    //         IsolationLevel.ReadCommitted);
    //     
    //     _context.ChangeTracker.Clear();
    //     
    //     var response2 = await _wrapper.ExecuteIdempotentTransaction(
    //         async (context) =>
    //         {
    //             var user = new User(0, 999, false);
    //             context.Users.Add(user);
    //             await context.SaveChangesAsync();
    //             return user;
    //         },
    //         "a",
    //         IsolationLevel.ReadCommitted);
    // }

    public void Dispose()
    {
        _scope.Dispose();
    }
}