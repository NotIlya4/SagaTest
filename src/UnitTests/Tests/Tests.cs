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
        _fixture.Bootstrapper.Bootstrap();
    }
    
    
    
    public void Dispose()
    {
        _scope.Dispose();
    }
}