using CatalogService.Interfaces;
using CatalogService.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// MongoDB configuration
var connectionString = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
var databaseName = builder.Configuration["MongoDB:DatabaseName"] ?? "CatalogDB";

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp => 
    sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

// Register repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
