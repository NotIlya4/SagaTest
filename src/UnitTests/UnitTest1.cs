using System.Data;
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

        _fixture.ReloadDb();
    }

    [Fact]
    public void Test()
    {
        _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
        
        _context.Idempotencies.Add("a");
        _context.Idempotencies.Add("b");
        _context.SaveChanges();
        
        _context.Database.RollbackTransaction();
    }
}