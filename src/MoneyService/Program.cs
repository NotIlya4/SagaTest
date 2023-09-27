using ExecutionStrategyExtended.Extensions;
using ExecutionStrategyExtended.Options;
using MoneyService;
using MoneyService.EntityFramework;
using MoneyService.ExtendedExecutionStrategyImpls;
using MoneyService.Extensions;
using MoneyService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;
var configurator = new DbConfigurator(DbType.Mssql, config);

services.AddAppContext(configurator);
services.AddScoped<UserCrud>();


services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();