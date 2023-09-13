using EfTest.Models;
using Microsoft.EntityFrameworkCore;
using AppContext = EfTest.EntityFramework.AppContext;

namespace UnitTests;

public class UnitTest1 : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly AppContext _context;

    public UnitTest1(TestFixture fixture)
    {
        _fixture = fixture;
        _context = fixture.Context;

        // _fixture.ReloadDb();
    }
    
    [Fact]
    public async Task Test1()
    {
        var (context1, _) = _fixture.CreateConnection();
        var (context2, _) = _fixture.CreateConnection();

        await context1.Database.BeginTransactionAsync();
        await context2.Database.BeginTransactionAsync();
        
        context1.Idempotencies.Add("a");
        await context1.SaveChangesAsync();

        context2.Idempotencies.Add("a");
        await context2.SaveChangesAsync();

        await context1.Database.CommitTransactionAsync();
    }

    [Fact]
    public async Task Test2()
    {
        var user1 = new User("1", "Boba");
        _context.Update(user1);

        _context.ChangeTracker.Clear();

        var users = await _context.Users.ToListAsync();
    }
}