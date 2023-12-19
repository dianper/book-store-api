using Domain.Entities;

namespace Application.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAsync(CancellationToken cancellationToken);

        ValueTask<Book?> GetAsync(int id, CancellationToken cancellationToken);

        Task CreateAsync(Book newBook, CancellationToken cancellationToken);
    }
}
