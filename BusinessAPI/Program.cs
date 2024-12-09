using BusinessAPI.Infrastructure;
using BusinessAPI.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis connection
var redisConnection = "redis:6379,abortConnect=false";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

// Business API services
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<CacheDataSource>();
builder.Services.AddScoped<UtcDateTimeConverter>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();
app.MapControllers();

app.Run();
