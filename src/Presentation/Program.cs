using Application;
using Infrastructure;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// Jwt Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services
    .AddApplicationServices() // IServices
    .AddInfrastructureRepositories() // IRepositories
    .AddMappers() // AutoMapper
    .AddQueryHandlers(); // MediatR

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithJwtConfiguration();

var app = builder.Build();

// Create Db
var db = app.Services.GetService<BookStoreDbContext>();
db.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
