using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(new Book("Book 1", 10, "Author 1", "Comedy") { Id = 1 });
        modelBuilder.Entity<Book>().HasData(new Book("Book 2", 15, "Author 2", "Action") { Id = 2 });
        modelBuilder.Entity<Book>().HasData(new Book("Book 3", 20, "Author 3", "History") { Id = 3 });
        modelBuilder.Entity<Book>().HasData(new Book("Book 4", 25, "Author 4", "Techonology") { Id = 4 });
    }

    public DbSet<Book> Books => Set<Book>();
}
