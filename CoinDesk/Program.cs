using CoinDesk.Data;
using CoinDesk.Handlers;
using CoinDesk.Middleware;
using CoinDesk.Repositories;
using CoinDesk.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<LoggingHttpMessageHandler>();
builder.Services.AddHttpClient("Logging").AddHttpMessageHandler<LoggingHttpMessageHandler>();
builder.Services.AddScoped<ICoinDeskRepository, CoinDeskRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICoinDeskService, CoinDeskService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var supportedCultures = new[] { "en-US", "zh-TW" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures.First())
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();