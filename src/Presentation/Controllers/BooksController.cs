using Application.Commands.Books.Commands;
using Application.Queries.Books.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Net;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<BooksController> logger;

    public BooksController(
        IMediator mediator,
        IMapper mapper,
        ILogger<BooksController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<Book>>> GetAsync(CancellationToken cancellationToken)
    {
        var books = await this.mediator.Send(new GetBooksQuery(), cancellationToken);

        return Ok(this.mapper.Map<List<Book>>(books));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Book>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var book = await this.mediator.Send(new GetBooksByIdQuery(id), cancellationToken);

        if (book is { }) return Ok(this.mapper.Map<Book>(book));

        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Book>> CreateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<CreateBookCommand>(book);

        var newBook = await this.mediator.Send(command, cancellationToken);

        if (newBook is { }) return Ok(this.mapper.Map<Book>(newBook));

        return BadRequest();
    }
}