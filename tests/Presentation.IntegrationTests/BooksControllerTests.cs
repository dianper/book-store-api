using Microsoft.AspNetCore.Mvc.Testing;
using Presentation.Models;
using System.Text.Json;

namespace Presentation.IntegrationTests
{
    [Trait("Category", "Controller Integration Tests")]
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
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/books/1");
            response.EnsureSuccessStatusCode();

            var book = JsonSerializer.Deserialize<Book>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(book);
            Assert.Equal(1, book.Id);
            Assert.Equal("Book 1", book.Name);
        }
    }
}
