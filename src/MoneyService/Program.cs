using ExecutionStrategyExtended;
using ExecutionStrategyExtended.Models;
using MoneyService.EntityFramework;
using MoneyService.Extensions;
using MoneyService.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddAppContext(config.GetPostgresConn());
services.AddScoped<UserCrud>();
services.AddTransient<ISerializer, Serializer>();
services.AddTransient<IClock, Clock>();
services.AddTransient<IIdempotencyViolationDetector, IdempotencyViolationDetector>();
services.AddTransient<IdempotencyFactory>();
services.AddScoped<ExecutionStrategyExtended<AppDbContext>>();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();