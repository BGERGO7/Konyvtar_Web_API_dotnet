using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using Microsoft.Extensions.DependencyModel;

namespace KonyvtarWebApi_BG.Models;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Borrow> Borrows { get; set; } = null!;
    public DbSet<LibraryCard> LibraryCards { get; set; } = null!;
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
    public DbSet<BookGenre> BookGenres { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(e => e.Authors)
            .WithMany(e => e.Books)
            .UsingEntity<BookAuthor>();


        modelBuilder.Entity<Book>()
            .HasMany(e => e.Genres)
            .WithMany(e => e.Books)
            .UsingEntity<BookGenre>();
    }
}