using Application.Queries.Todos.Queries;
using Domain.External;
using Infrastructure.ExternalServices;
using MediatR;

namespace Application.Queries.Todos.Handlers;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IEnumerable<Todo>>
{
    private readonly TodoService todoService;

    public GetTodosQueryHandler(TodoService todoService)
    {
        this.todoService = todoService;
    }

    public Task<IEnumerable<Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return this.todoService.GetTodosAsync(cancellationToken);
    }
}
