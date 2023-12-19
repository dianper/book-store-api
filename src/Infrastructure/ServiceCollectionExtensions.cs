using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
    {
        services.AddDbContext<BookStoreDbContext>(options => options.UseInMemoryDatabase("BookStoreDb"), ServiceLifetime.Singleton);
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }
}
