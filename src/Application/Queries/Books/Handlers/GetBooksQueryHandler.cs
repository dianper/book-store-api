using Application.Queries.Books.Queries;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Queries.Books.Handlers;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<Book>>
{
    private readonly IBookRepository bookRepository;

    public GetBooksQueryHandler(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public Task<List<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return this.bookRepository
            .GetAsync(cancellationToken);
    }
}
