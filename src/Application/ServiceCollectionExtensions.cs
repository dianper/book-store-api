using Application.Commands.Books.Handlers;
using Application.Queries.Books.Handlers;
using Application.Queries.Todos.Handlers;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetBooksQueryHandler)))
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetBooksByIdQueryHandler)))
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetTodosByUserIdQueryHandler)))
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetTodosQueryHandler)));

        return services;
    }

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateBookCommandHandler)));

        return services;
    }
}
