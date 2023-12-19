using Domain.Entities;
using MediatR;

namespace Application.Queries.Books.Queries;

public record GetBooksByIdQuery(int Id) : IRequest<Book>;
