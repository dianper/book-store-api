using System.Reflection.PortableExecutable;

namespace Presentation.Middlewares;

public class RequestHeadersMiddleware
{
    private readonly RequestDelegate next;

    private readonly ILogger<RequestHeadersMiddleware> logger;

    private const string RequiredHeader = "MyRequiredHeader";

    public RequestHeadersMiddleware(RequestDelegate next, ILogger<RequestHeadersMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var isShellEnabled = configuration.GetSection("Shell").GetValue<bool>("Enabled");

        if (!isShellEnabled && !context.Request.Headers.ContainsKey(RequiredHeader))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var messageResult = $"Required header '{RequiredHeader}' is missing.";

            await context.Response.WriteAsync(messageResult);

            this.logger.LogWarning(messageResult, new { A = 1 });

            return;
        }

        await this.next(context);
    }
}
