using System.Text.Json.Serialization;
using NLog.Extensions.Logging;
using WorldRank.Application;
using WorldRank.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

var connectionString = builder.Configuration.GetConnectionString("WorldRank")
	?? throw new InvalidOperationException("Connection string not found.");
var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase");

builder.Services.AddInfrastructure(connectionString, useDatabase);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICache, MemoryCacheStore>();

builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddControllers()
	.AddJsonOptions(options =>
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
