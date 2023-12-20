using Domain.External;
using MediatR;

namespace Application.Queries.Todos.Queries;

public record GetTodosByUserIdQuery(int Id) : IRequest<IEnumerable<Todo>>;
