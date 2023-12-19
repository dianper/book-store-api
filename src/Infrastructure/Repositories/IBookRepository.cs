using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAsync(CancellationToken cancellationToken);

    ValueTask<Book?> GetAsync(int id, CancellationToken cancellationToken);

    Task<Book> CreateAsync(Book newBook, CancellationToken cancellationToken);
}
