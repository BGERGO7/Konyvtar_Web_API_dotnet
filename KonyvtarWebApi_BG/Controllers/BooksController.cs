using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Book;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDto>>> GetBooks()
        {
            return await _context.Books
                .Include(b=> b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Select(book => new BookReadDto
                {
                    BookId = book.BookId,
                    HungarianTitle = book.HungarianTitle,
                    OriginalTitle = book.OriginalTitle,
                    RecommendationText = book.RecommendationText,
                    PublishedYear = book.PublishedYear,
                    MaxInventoryCount = book.MaxInventoryCount,
                    CurrentInventoryCount = book.CurrentInventoryCount,
                    Publisher = book.Publisher,
                    MaxRentDays = book.MaxRentDays,
                    Active = book.Active,
                    Created = book.Created,
                    Modified = book.Modified,
                    Authors = book.BookAuthors.Select(ba => ba.Author.AuthorName).ToList(),
                    Genres = book.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return new BookReadDto
            {
                BookId = book.BookId,
                HungarianTitle = book.HungarianTitle,
                OriginalTitle = book.OriginalTitle,
                RecommendationText = book.RecommendationText,
                PublishedYear = book.PublishedYear,
                MaxInventoryCount = book.MaxInventoryCount,
                CurrentInventoryCount = book.CurrentInventoryCount,
                Publisher = book.Publisher,
                MaxRentDays = book.MaxRentDays,
                Active = book.Active,
                Created = book.Created,
                Modified = book.Modified,
                Authors = book.BookAuthors.Select(ba => ba.Author.AuthorName).ToList(),
                Genres = book.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
            };
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {

            if (id != bookDto.BookId)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.BookId = bookDto.BookId;
            book.HungarianTitle = bookDto.HungarianTitle;
            book.OriginalTitle = bookDto.OriginalTitle;
            book.RecommendationText = bookDto.RecommendationText;
            book.PublishedYear = bookDto.PublishedYear;
            book.MaxInventoryCount = bookDto.MaxInventoryCount;
            book.CurrentInventoryCount = bookDto.CurrentInventoryCount;
            book.Publisher = bookDto.Publisher;
            book.MaxRentDays = bookDto.MaxRentDays;
            book.Active = bookDto.Active;
            book.Created = bookDto.Created;
            book.Modified = DateTime.UtcNow;
            
            
            book.Authors = bookDto.AuthorIds
                .Select(authorId => _context.Authors.Find(authorId))
                .Where(author => author != null)
                .ToList()!;
            book.Genres = bookDto.GenreIds
                .Select(genreId => _context.Genres.Find(genreId))
                .Where(genre => genre != null)
                .ToList()!;
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {

            if(bookDto.CurrentInventoryCount < 0 || bookDto.CurrentInventoryCount > bookDto.MaxInventoryCount)
            {
                return BadRequest("CurrentInventoryCount must be between 0 and MaxInventoryCount.");
            }

            var book = new Book
            {
                HungarianTitle = bookDto.HungarianTitle,
                OriginalTitle = bookDto.OriginalTitle,
                RecommendationText = bookDto.RecommendationText,
                PublishedYear = bookDto.PublishedYear,
                MaxInventoryCount = bookDto.MaxInventoryCount,
                CurrentInventoryCount = bookDto.CurrentInventoryCount,
                Publisher = bookDto.Publisher,
                MaxRentDays = bookDto.MaxRentDays,
                Active = bookDto.Active,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
