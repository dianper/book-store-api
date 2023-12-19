using Application.Commands.Books.Commands;
using Application.Queries.Books.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Presentation.Controllers;
using System.Net;

namespace Presentation.Tests
{
    [Trait("Category", "Unit Tests")]
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ILogger<BooksController>> loggerMock;

        public BooksControllerTests()
        {
            this.mediatorMock = new Mock<IMediator>();
            this.mapperMock = new Mock<IMapper>();
            this.loggerMock = new Mock<ILogger<BooksController>>();
        }

        [Fact]
        public async Task BooksController_GetBooks_ReturnsBooks()
        {
            // Arrange
            this.mediatorMock
                .Setup(s => s.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Entities.Book>
                {
                    new Domain.Entities.Book("BookName", 10, "Author", "Category") { Id = 1 },
                });

            this.mapperMock
                .Setup(s => s.Map<List<Models.Book>>(It.IsAny<List<Domain.Entities.Book>>()))
                .Returns(new List<Models.Book>
                {
                    new Models.Book(1, "BookName", 10, "Author", "Category")
                });

            var controller = new BooksController(this.mediatorMock.Object, this.mapperMock.Object, this.loggerMock.Object);

            // Act
            var action = await controller.GetAsync(It.IsAny<CancellationToken>());
            var result = action.Result as OkObjectResult;
            var books = result?.Value as List<Models.Book>;

            // Assert
            Assert.NotNull(books);
            Assert.NotEmpty(books);
        }

        [Fact]
        public async Task BooksController_CreateNewBook_ReturnsOk()
        {
            // Arrange
            this.mapperMock
                .Setup(s => s.Map<CreateBookCommand>(It.IsAny<Models.Book>()))
                .Returns(new CreateBookCommand("NewBook", 20, "Author", "Category"));

            this.mediatorMock
                .Setup(s => s.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Book("NewBook", 10, "Author", "Category") { Id = 9 });

            this.mapperMock
                .Setup(s => s.Map<Models.Book>(It.IsAny<Domain.Entities.Book>()))
                .Returns(new Models.Book(9, "NewBook", 20, "Author", "Category"));

            var controller = new BooksController(this.mediatorMock.Object, this.mapperMock.Object, this.loggerMock.Object);

            // Act
            var action = await controller.CreateBookAsync(It.IsAny<Models.Book>(), It.IsAny<CancellationToken>());
            var result = action.Result as OkObjectResult;
            var book = result?.Value as Models.Book;

            // Assert
            Assert.NotNull(book);
            Assert.Equal(9, book.Id);
        }

        [Fact]
        public async Task BooksController_CreateNewBook_ReturnsBadRequest()
        {
            // Arrange
            this.mapperMock
                .Setup(s => s.Map<CreateBookCommand>(It.IsAny<Models.Book>()))
                .Returns(new CreateBookCommand("NewBook", 20, "Author", "Category"));

            this.mediatorMock
                .Setup(s => s.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Domain.Entities.Book));

            var controller = new BooksController(this.mediatorMock.Object, this.mapperMock.Object, this.loggerMock.Object);

            // Act
            var action = await controller.CreateBookAsync(It.IsAny<Models.Book>(), It.IsAny<CancellationToken>());
            var result = action.Result as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}