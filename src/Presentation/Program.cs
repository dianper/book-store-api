using Application;
using Infrastructure;
using Infrastructure.ExternalServices;
using Polly;
using Polly.Extensions.Http;
using Presentation;
using Presentation.Middlewares;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// HttpClient & Polly
builder.Services
    .AddHttpClient(builder.Configuration);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Memory Cache
builder.Services
    .AddMemoryCache();

// Health Check
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<BookStoreDbContext>();

// Jwt Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services
    .AddApplicationServices() // IServices
    .AddInfrastructureRepositories() // IRepositories
    .AddMappers() // AutoMapper
    .AddQueryHandlers() // MediatR Queries
    .AddCommandHandlers(); // MediatR Commands

builder.Services
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithJwtConfiguration();

var app = builder.Build();

// Request Headers
app.UseMiddleware<RequestHeadersMiddleware>();
app.UseMiddleware<MetricsMiddleware>();

// Create Db
var db = app.Services.GetService<BookStoreDbContext>();
db.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/monitoring/ping");

app.Run();

// Partial class is used in the Integration Tests
public partial class Program { }
