using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public Task CreateAsync(Book newBook, CancellationToken cancellationToken)
    {
        return this.bookRepository.CreateAsync(newBook, cancellationToken);
    }

    public Task<List<Book>> GetAsync(CancellationToken cancellationToken)
    {
        return this.bookRepository.GetAsync(cancellationToken);
    }

    public ValueTask<Book?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return this.bookRepository.GetAsync(id, cancellationToken);
    }
}
