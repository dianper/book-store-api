using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookStoreDbContext bookStoreDbContext;

    public BookRepository(BookStoreDbContext bookStoreDbContext)
    {
        this.bookStoreDbContext = bookStoreDbContext;
    }

    public async Task<Book> CreateAsync(Book newBook, CancellationToken cancellationToken)
    {
        this.bookStoreDbContext.Books.Add(newBook);
        await this.bookStoreDbContext.SaveChangesAsync(cancellationToken);
        return newBook;
    }

    public Task<List<Book>> GetAsync(CancellationToken cancellationToken)
    {
        return this.bookStoreDbContext.Books.ToListAsync(cancellationToken);
    }

    public ValueTask<Book?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return this.bookStoreDbContext.Books.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }
}
