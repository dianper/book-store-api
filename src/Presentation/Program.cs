using Application;
using Application.Queries.Books.Handlers;
using Infrastructure;
using Presentation;
using Presentation.Mapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices() // IServices
    .AddInfrastructureRepositories() // IRepository
    .AddMappers() // AutoMapper
    .AddQueryHandlers(); // MediatR

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
