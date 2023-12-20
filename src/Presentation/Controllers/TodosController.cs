using Application.Queries.Todos.Queries;
using Domain.External;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ILogger<TodosController> logger;

    public TodosController(IMediator mediator, ILogger<TodosController> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Todo>), 200)]
    public async Task<IActionResult> GetTodosAsync(CancellationToken cancellationToken)
    {
        var todos = await this.mediator.Send(new GetTodosQuery(), cancellationToken);

        return Ok(todos);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(IEnumerable<Todo>), 200)]
    public async Task<IActionResult> GetTodosAsync(int userId, CancellationToken cancellationToken)
    {
        var todos = await this.mediator.Send(new GetTodosByUserIdQuery(userId), cancellationToken);

        return Ok(todos);
    }
}
