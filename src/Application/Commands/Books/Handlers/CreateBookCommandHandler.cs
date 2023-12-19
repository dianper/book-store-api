using Application.Commands.Books.Commands;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Commands.Books.Handlers;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
{
    private readonly IBookRepository bookRepository;

    public CreateBookCommandHandler(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        return this.bookRepository.CreateAsync(new Domain.Entities.Book(
            request.Name,
            request.Price,
            request.Author,
            request.Category), cancellationToken);
    }
}
