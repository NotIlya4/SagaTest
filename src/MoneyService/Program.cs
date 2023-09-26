using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Options;
using MoneyService.EntityFramework;
using MoneyService.ExtendedExecutionStrategyImpls;
using MoneyService.Extensions;
using MoneyService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddAppContext(config.GetPostgresConn());
services.AddScoped<UserCrud>();

services.AddExecutionStrategyExtended<AppDbContext>(options =>
{
    options.ResponseSerializer.Use(new ResponseSerializer());
});

LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();