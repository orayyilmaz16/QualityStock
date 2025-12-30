using Microsoft.EntityFrameworkCore;
using QualityStock.Application.Abstractions;
using QualityStock.Application.Services;
using QualityStock.Infrastructure;
using QualityStock.Infrastructure.Automation;
using QualityStock.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<QualityStockDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// DI: Repos + UoW
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// DI: Services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<StockService>();

// DI: System
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddSingleton<INotifier, ConsoleNotifier>();

// Automation worker
builder.Services.AddHostedService<StockQualityAutomationWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
