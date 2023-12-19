using Domain.Entities;
using MediatR;

namespace Application.Commands.Books.Commands;

public record CreateBookCommand(string Name, decimal Price, string Author, string Category) : IRequest<Book>;
