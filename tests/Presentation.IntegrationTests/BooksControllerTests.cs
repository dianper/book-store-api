using Microsoft.AspNetCore.Mvc.Testing;
using Presentation.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Presentation.IntegrationTests
{
    public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BooksControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task BooksController_GetBookById_ReturnsBook()
        {
            // Arrange
            Environment.SetEnvironmentVariable("Shell:Enabled", "true");

            var client = _factory.CreateClient();
            var token = await this.GetJwtTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/api/books/1");
            response.EnsureSuccessStatusCode();

            var book = JsonSerializer.Deserialize<Book>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(book);
            Assert.Equal(1, book!.Id);
            Assert.Equal("Book 1", book.Name);
        }

        private async Task<string?> GetJwtTokenAsync(HttpClient client)
        {
            var loginModel = new
            {
                Username = "username"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/auth", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return token?.Token;
        }
    }

    public record TokenModel
    {
        public string? Token { get; set; }
    }
}