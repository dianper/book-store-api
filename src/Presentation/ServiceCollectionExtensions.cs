using Presentation.Mapper;
using System.Reflection;

namespace Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(typeof(BookMapper).GetTypeInfo().Assembly);

        return services;
    }
}
