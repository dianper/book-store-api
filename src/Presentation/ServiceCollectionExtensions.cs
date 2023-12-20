using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Presentation.Mapper;
using System.Reflection;
using System.Text;

namespace Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(typeof(BookMapper).GetTypeInfo().Assembly);

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration.GetSection("Jwt:Key").Value;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static IServiceCollection AddSwaggerGenWithJwtConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultTimeout = configuration.GetValue<int>("HttpClientDefaultTimeout");
        var todoApiConfigs = configuration.GetSection("TodoApi");
        var timeoutPolicy = todoApiConfigs.GetSection("ResiliencePolices:Timeout");
        var retryPolicy = todoApiConfigs.GetSection("ResiliencePolices:Retry");
        var circuitBreakerPolicy = todoApiConfigs.GetSection("ResiliencePolices:CircuitBreaker");

        services.AddHttpClient<TodoService>(client =>
        {
            client.BaseAddress = new Uri(todoApiConfigs["BaseAddress"]);
            client.Timeout = TimeSpan.FromMilliseconds(defaultTimeout);
        })
        .AddPolicyHandler(policy =>
        {
            return Policy
                .TimeoutAsync<HttpResponseMessage>(timeoutPolicy.GetValue<int>("Value"));
        })
        .AddPolicyHandler(policy =>
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(retryPolicy.GetValue<int>("Count"), retryAttempt => TimeSpan.FromSeconds(retryAttempt));
        })
        .AddPolicyHandler(policy =>
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    circuitBreakerPolicy.GetValue<int>("handledEventsAllowedBeforeBreaking"), 
                    TimeSpan.FromSeconds(circuitBreakerPolicy.GetValue<int>("durationOfBreak")));
        });

        return services;
    }
}
