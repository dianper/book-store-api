using Application.Queries.Todos.Queries;
using Domain.External;
using Infrastructure.ExternalServices;
using MediatR;

namespace Application.Queries.Todos.Handlers;

public class GetTodosByUserIdQueryHandler : IRequestHandler<GetTodosByUserIdQuery, IEnumerable<Todo>>
{
    private readonly TodoService todoService;

    public GetTodosByUserIdQueryHandler(TodoService todoService)
    {
        this.todoService = todoService;
    }

    public Task<IEnumerable<Todo>> Handle(GetTodosByUserIdQuery request, CancellationToken cancellationToken)
    {
        return this.todoService.GetTodosByUserIdAsync(request.Id, cancellationToken);
    }
}
