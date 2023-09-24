using ExecutionStrategyExtended;
using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Models;
using ExecutionStrategyExtended.Options;
using ExecutionStrategyExtended.Utils;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using MoneyService.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddAppContext(config.GetPostgresConn());
services.AddScoped<UserCrud>();
services.AddIdempotentTransactionServices<AppDbContext>(options =>
{
    options
        .SystemClock.Use(new SystemClock())
        .BetweenRetriesBehavior.Configure(behavior => behavior.WithRetryPolicy(DbContextRetryPolicy.CreateNew));
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

var value = app.Services.CreateScope().ServiceProvider.GetRequiredService<IdempotentTransactionOptions>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();