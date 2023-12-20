using Domain.External;
using MediatR;

namespace Application.Queries.Todos.Queries;

public class GetTodosQuery : IRequest<IEnumerable<Todo>>
{

}
