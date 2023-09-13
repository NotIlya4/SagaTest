using EfTest.Models;
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
    public async Task Test1()
    {
        var idempotency1 = new Idempotency("1");
        var idempotency2 = new Idempotency("2");
        _context.Idempotencies.Add(idempotency1);
        _context.Idempotencies.Add(idempotency2);

        await _context.SaveChangesAsync();
    }
}