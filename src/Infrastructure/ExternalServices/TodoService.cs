using Domain.External;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.ExternalServices;

public class TodoService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TodoService> logger;

    public TodoService(
        HttpClient httpClient,
        ILogger<TodoService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<IEnumerable<Todo>> GetTodosAsync(CancellationToken cancellationToken)
    {
        try
        {
            var todos = await this.httpClient
                .GetFromJsonAsync<IEnumerable<Todo>>(
                    requestUri: "todos",
                    options: new JsonSerializerOptions(JsonSerializerDefaults.Web),
                    cancellationToken);

            return todos ?? Enumerable.Empty<Todo>();
        }
        catch (Exception ex)
        {
            this.logger.LogError("Error getting data: {Error}", ex);
        }

        return Enumerable.Empty<Todo>();
    }

    public async Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var todos = await this.httpClient
                .GetFromJsonAsync<IEnumerable<Todo>>(
                    requestUri: $"todos?userId={userId}",
                    options: new JsonSerializerOptions(JsonSerializerDefaults.Web),
                    cancellationToken);

            return todos ?? Enumerable.Empty<Todo>();
        }
        catch (Exception ex)
        {
            this.logger.LogError("Error getting data: {Error}", ex);
        }

        return Enumerable.Empty<Todo>();
    }
}
