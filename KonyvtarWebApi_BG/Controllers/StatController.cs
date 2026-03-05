using KonyvtarWebApi_BG.DTOs.Book;
using KonyvtarWebApi_BG.DTOs.Stats;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Author;
using KonyvtarWebApi_BG.DTOs.Genre;
using KonyvtarWebApi_BG.DTOs.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        private readonly LibraryContext _context;

        public StatController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Stat/topbooks/5
        [HttpGet("topbooks/{count}")]
        public async Task<ActionResult<IEnumerable<BookTopBorrowDto>>> GetTopBooks(int count)
        {
            if (count <= 0)
            {
                return BadRequest("A megadott szám nagyobb legyen, mint 0!");
            }

            var topBooks = await _context.Borrows
                .Include(l => l.Book)
                    .ThenInclude(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(l => l.Book)
                    .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Where(
                    l => l.ReturnDate != null
                    && l.Active
                    && l.Book != null
                    && l.Book.Active
                    && l.Book.BookAuthors.Any()
                    && l.Book.BookAuthors.All(ba => ba.Active)
                    && l.Book.BookGenres.Any()
                    && l.Book.BookGenres.All(bg => bg.Active)
                    )
                .GroupBy(l => l.Book)
                .Select(g => new BookTopBorrowDto
                {
                    BookId = g.Key.BookId,
                    HungarianTitle = g.Key.HungarianTitle,
                    OriginalTitle = g.Key.OriginalTitle,
                    Authors = g.Key.BookAuthors.Select(ba => new AuthorForStatDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.AuthorName
                    }).ToList(),
                    Genres = g.Key.BookGenres.Select(bg => new GenreForStatDto
                    {
                        GenreId = bg.Genre.GenreId,
                        Name = bg.Genre.GenreName
                    }).ToList(),
                    TotalBorrows = g.Count()
                })
                .OrderByDescending(b => b.TotalBorrows)
                .Take(count)
                .ToListAsync();

            return topBooks;
        }

        // GET: api/Stat/topstudents/5
        [HttpGet("topstudent/{count}")]
        public async Task<ActionResult<IEnumerable<StudentTopBorrowDto>>> GetTopStudents(int count)
        {
            if (count <= 0)
            {
                return BadRequest("A megadott szám nagyobb legyen, mint 0!");
            }

            var topStudents = await _context.Borrows
                .Include(l => l.Student)
                .Where(
                    l => l.ReturnDate != null
                    && l.Active
                    && l.Student != null
                    && l.Student.Active
                    )
                .GroupBy(l => l.Student)
                .Select(g => new StudentTopBorrowDto
                {
                    StudentId = g.Key.StudentId,
                    StudentName = g.Key.StudentName,
                    Class = g.Key.Class,
                    EmailAddress = g.Key.EmailAddress,
                    TotalBorrows = g.Count()
                })
                .OrderByDescending(s => s.TotalBorrows)
                .Take(count)
                .ToListAsync();

            return topStudents;
        }

        // GET: api/Stat/currentloanbooks
        [HttpGet("currentloanbooks")]
        public async Task<ActionResult<IEnumerable<CurrentlyBorrowedBookDto>>> GetCurrentLoanBooks()
        {
            var currentLoanBooks = await _context.Borrows
                .Include(l => l.Book)
                    .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(l => l.Book)
                    .ThenInclude(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(l => l.Student)
                .Where(
                        l => l.ReturnDate == null
                        && l.Active
                        && l.Student != null
                        && l.Student.Active
                        && l.Book.Active
                        && l.Book.BookAuthors.Any()
                        && l.Book.BookAuthors.All(ba => ba.Active)
                        && l.Book.BookGenres.Any()
                        && l.Book.BookGenres.All(bg => bg.Active)
                    )
                .Select(l => new CurrentlyBorrowedBookDto
                {
                    BookId = l.Book.BookId,
                    HungarianTitle = l.Book.HungarianTitle,
                    OriginalTitle = l.Book.OriginalTitle,
                    DueDate = l.DueDate,
                    IsOverdue = l.DueDate < DateTime.Now,
                    Student = new StudentForStatDto
                    {
                        StudentId = l.Student.StudentId,
                        Name = l.Student.StudentName,
                        Class = l.Student.Class,
                        EmailAddress = l.Student.EmailAddress
                    },
                    Authors = l.Book.BookAuthors.Select(ba => new AuthorForStatDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.AuthorName
                    }).ToList(),
                    Genres = l.Book.BookGenres.Select(bg => new GenreForStatDto
                    {
                        GenreId = bg.Genre.GenreId,
                        Name = bg.Genre.GenreName
                    }).ToList()
                })
                .ToListAsync();

            return currentLoanBooks;
        }

        // Elérhető könyvek listája
        // GET: api/Stat/availablebooks
        [HttpGet("availablebooks")]
        public async Task<ActionResult<IEnumerable<AvailableBooksDto>>> GetAvailableBooks()
        {
            var availableBooks = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Where(
                    b => b.Active
                    && b.BookAuthors.Any()
                    && b.BookAuthors.All(ba => ba.Active)
                    && b.BookGenres.Any()
                    && b.BookGenres.All(bg => bg.Active)
                    && b.CurrentInventoryCount > 0
                    )
                .Select(b => new AvailableBooksDto
                {
                    Id = b.BookId,
                    HungarianTitle = b.HungarianTitle,
                    OriginalTitle = b.OriginalTitle,
                    Authors = b.BookAuthors.Select(ba => new AuthorForStatDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.AuthorName
                    }).ToList(),
                    Genres = b.BookGenres.Select(bg => new GenreForStatDto
                    {
                        GenreId = bg.Genre.GenreId,
                        Name = bg.Genre.GenreName
                    }).ToList(),
                    AvailableNumberOfCopies = b.CurrentInventoryCount
                })
                .ToListAsync();

            return availableBooks;
        }

        // Műfaj szerinti könyv lista
        // GET: api/Stat/booksbygenre/5
        [HttpGet("booksbygenre/{Id}")]
        public async Task<ActionResult<BookByGenreDto>> GetBookByGenre(int Id)
        {
            var genre = await _context.Genres.FindAsync(Id);

            if (genre == null) return NotFound();

            var books = await _context.Books
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Where(
                    b => b.Active
                    && b.BookGenres.Any(bg => bg.GenreId == Id && bg.Active)
                    && b.BookAuthors.Any()
                    && b.BookAuthors.All(ba => ba.Active)
                    && b.BookGenres.Any()
                    && b.BookGenres.All(bg => bg.Active))
                .Select(b => new BookStatItemDto
                {
                    Id = b.BookId,
                    HungarianTitle = b.HungarianTitle,
                    OriginalTitle = b.OriginalTitle,
                    Authors = b.BookAuthors.Select(ba => new AuthorForStatDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.AuthorName
                    }).ToList(),
                    Genres = b.BookGenres.Select(bg => new GenreForStatDto
                    {
                        GenreId = bg.Genre.GenreId,
                        Name = bg.Genre.GenreName
                    }).ToList(),
                    TotalCopies = b.MaxInventoryCount,
                    PublicationYear = b.PublishedYear
                }).ToListAsync();

            return new BookByGenreDto
            {
                Genre = new GenreForStatDto
                {
                    GenreId = genre.GenreId,
                    Name = genre.GenreName
                },
                Books = books
            };

        }

        // Szerző szerinti könyv lista
        // GET: api/Stat/booksbyauthor/5
        [HttpGet("booksbyauthor/{Id}")]
        public async Task<ActionResult<AuthorGetBooksDto>> GetBookByAuthor(int Id)
        {
            var author = await _context.Authors.FindAsync(Id);

            if (author == null) return NotFound();

            var books = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Where(
                    b => b.Active
                    && b.BookAuthors.Any(ba => ba.AuthorId == Id && ba.Active)
                    && b.BookAuthors.Any()
                    && b.BookAuthors.All(ba => ba.Active)
                    && b.BookGenres.Any()
                    && b.BookGenres.All(bg => bg.Active))
                .Select(b => new BookStatItemDto
                {
                    Id = b.BookId,
                    HungarianTitle = b.HungarianTitle,
                    OriginalTitle = b.OriginalTitle,
                    Authors = b.BookAuthors.Select(ba => new AuthorForStatDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.AuthorName
                    }).ToList(),
                    Genres = b.BookGenres.Select(bg => new GenreForStatDto
                    {
                        GenreId = bg.Genre.GenreId,
                        Name = bg.Genre.GenreName
                    }).ToList(),
                    TotalCopies = b.MaxInventoryCount,
                    PublicationYear = b.PublishedYear
                }).ToListAsync();

            return new AuthorGetBooksDto
            {
                Author = new AuthorForStatDto
                {
                    AuthorId = author.AuthorId,
                    Name = author.AuthorName
                },
                Books = books
            };
        }
    }
}
