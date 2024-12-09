using DataAPI.Infrastructure.CurrencyRepository;
using DataAPI.Infrastructure.GenericRepository;
using DataAPI.Infrastructure;
using DataAPI.Services.CurrencyServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DataAPI.Services.MigrationServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DotnetCurrencyProjectContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});



builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<MigrationService>();

var app = builder.Build();
MigrationService.InitializeMigration(app);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
});

app.UseAuthorization();
app.MapControllers();

app.Run();
