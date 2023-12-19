using Domain.Entities;
using MediatR;

namespace Application.Queries.Books.Queries;

public class GetBooksQuery : IRequest<List<Book>>
{

}
