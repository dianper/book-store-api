using Application.Queries.Books.Queries;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Queries.Books.Handlers;

public class GetBooksByIdQueryHandler : IRequestHandler<GetBooksByIdQuery, Book?>
{
    private readonly IBookRepository bookRepository;

    public GetBooksByIdQueryHandler(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public Task<Book?> Handle(GetBooksByIdQuery request, CancellationToken cancellationToken)
    {
        return this.bookRepository
            .GetAsync(request.Id, cancellationToken)
            .AsTask();
    }
}
