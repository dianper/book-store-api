using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Presentation.HealthChecks
{
    public class TodoApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<TodoApiHealthCheck> logger;

        public TodoApiHealthCheck(HttpClient httpClient, ILogger<TodoApiHealthCheck> logger)
        {

            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Make a request to the external API
                var response = await this.httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("External API is reachable and healthy.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"External API returned status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Failed to reach the external API. Error: {ex.Message}");
            }
        }
    }
}
