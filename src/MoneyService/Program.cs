using ExecutionStrategyExtended.Options;
using MoneyService.Extensions;
using MoneyService.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddAppContext(config.GetPostgresConn());
services.AddScoped<UserCrud>();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

var value = app.Services.CreateScope().ServiceProvider.GetRequiredService<IdempotentTransactionOptions>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();