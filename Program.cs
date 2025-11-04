using CatalogService.Interfaces;
using CatalogService.Repositories;
using MongoDB.Driver;
using NLog;
using NLog.Web;

try
{
    var logger =
NLog.LogManager.Setup().LoadConfigurationFromAppSettings()
          .GetCurrentClassLogger();
    logger.Debug("start min service");
}
catch (Exception ex)
{
    Console.WriteLine($"NLog setup failed: {ex.Message}");
    throw;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// MongoDB configuration
var connectionString = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
var databaseName = builder.Configuration["MongoDB:DatabaseName"] ?? "ProductDB";

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp => 
    sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

// Register repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Use NLog from here on
builder.Logging.ClearProviders();
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
}).UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseCors(policy => policy
   .SetIsOriginAllowed(origin =>
   {
       if (string.IsNullOrEmpty(origin)) return false;
       try { return new Uri(origin).Host == "localhost"; }
       catch { return false; }
   })
   .AllowAnyHeader()
   .AllowAnyMethod()
   .AllowCredentials()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
