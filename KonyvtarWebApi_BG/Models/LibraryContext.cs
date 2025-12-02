using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;

namespace KonyvtarWebApi_BG.Models;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
}