using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Book;
using KonyvtarWebApi_BG.DTOs.Stats;

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
            var books = await _context.Books
                .Include(b => b.BookAuthors.Where(ba=> ba.Active && ba.Author.Active))
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres.Where(b=> b.Active && b.Genre.Active))
                .ThenInclude(bg => bg.Genre)
                .ToListAsync();


            return books.Select(b => MapToDto(b)).ToList();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors
                    .Where(ba => ba.Active && ba.Author.Active))
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres
                    .Where(bg => bg.Active && bg.Genre.Active))
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return MapToDto(book);
        }

        // GET: api/Books/{id}/borrows
        [HttpGet("{id}/borrows")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookBorrowHistory(int id)
        {
            var book = await _context.Books
                .Include(b => b.Borrows)
                .ThenInclude(bor => bor.Student) // Betöltjük a diákot is
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound("A megadott könyv nem található.");
            }

            var history = book.Borrows
                .OrderByDescending(b => b.BorrowDate) // Legfrissebbek elöl
                .Select(b => new BookBorrowHistoryDto
                {
                    BorrowId = b.BorrowId,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate,
                    StudentId = b.Student.StudentId,
                    StudentName = b.Student.StudentName,
                    StudentClass = b.Student.Class // Feltételezve, hogy van 'Class' property a Student-en
                })
                .ToList();

            return Ok(history);
        }

        // GET: api/Books/currentlyBorrowed
        [HttpGet("currentlyBorrowed")]
        public async Task<ActionResult<IEnumerable<CurrentlyBorrowedBookDto>>> GetCurrentlyBorrowedBooks()
        {
            var borrowedBooks = await _context.Borrows // Biztonság kedvéért Set<Borrow>(), ha nincs direkt Borrows property
                .Include(b => b.Book)
                .Include(b => b.Student)
                .Where(b => b.ReturnDate == null) // Csak azokat kérjük le, amiket még nem hoztak vissza
                .OrderBy(b => b.DueDate)          // A lejárathoz legközelebb esők (vagy már lejártak) legyenek elöl
                .Select(b => new CurrentlyBorrowedBookDto
                {
                    BorrowId = b.BorrowId,
                    BookId = b.BookId,
                    HungarianTitle = b.Book.HungarianTitle,
                    StudentName = b.Student.StudentName,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    // Ha a mai dátum nagyobb mint a határidő, akkor lejártnak tekintjük
                    IsOverdue = DateTime.Now > b.DueDate
                })
                .ToListAsync();

            return Ok(borrowedBooks);
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

            var now = DateTime.Now;

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            // Frissítés
            book.HungarianTitle = bookDto.HungarianTitle;
            book.OriginalTitle = bookDto.OriginalTitle;
            book.RecommendationText = bookDto.RecommendationText;
            book.PublishedYear = bookDto.PublishedYear;
            book.MaxInventoryCount = bookDto.MaxInventoryCount;
            book.Publisher = bookDto.Publisher;
            book.MaxRentDays = bookDto.MaxRentDays;
            book.Modified = now;
            book.Active = bookDto.Active;

            // Szerzők és műfajok frissítése
            // Műfajok

            if (bookDto.GenreIds != null && bookDto.GenreIds.Count > 0)
            {

                var existingGenreIds = book.BookGenres
                    .Where(bg => bg.Active)
                    .Select(bg => bg.GenreId)
                    .ToList();

                var newValidGenreIds = await _context.Genres
                    .Where(g => bookDto.GenreIds.Contains(g.GenreId) && g.Active)
                    .Select(g => g.GenreId)
                    .ToListAsync();

                if (newValidGenreIds == null || newValidGenreIds.Count == 0)
                {
                    return BadRequest("Nem lett érvényes műfaj megadva!");
                }

                // Új műfajok hozzáadása

                foreach (var gid in newValidGenreIds)
                {
                    var existingRelation = book.BookGenres
                                            .FirstOrDefault(bg => bg.GenreId == gid);

                    if (existingRelation == null)
                    {
                        book.BookGenres.Add(new BookGenre
                        {
                            BookId = book.BookId,
                            GenreId = gid,
                            Created = now,
                            Modified = now,
                            Active = true
                        });
                    }
                    else if (!existingRelation.Active)
                    {
                        // Ha korábban inaktív volt, akkor újraaktiváljuk
                        existingRelation.Active = true;
                        existingRelation.Modified = now;
                    }
                }

                // Műfajok törlése (IsActive=false)

                foreach (var gid in existingGenreIds)
                {
                    if (!bookDto.GenreIds.Contains(gid))
                    {
                        var bookGenre = book.BookGenres.FirstOrDefault(bg => bg.GenreId == gid);

                        if (bookGenre != null)
                        {
                            bookGenre.Active = false;
                            bookGenre.Modified = now;
                        }
                    }
                }
            }

            // Szerzők kezelése

            if (bookDto.AuthorIds != null && bookDto.AuthorIds.Count > 0)
            {
                var existingAuthorIds = book.BookAuthors
                    .Where(ba => ba.Active)
                    .Select(ba => ba.AuthorId)
                    .ToList();

                var newValidAuthorIds = await _context.Authors
                    .Where(a => bookDto.AuthorIds.Contains(a.AuthorId) && a.Active)
                    .Select(a => a.AuthorId)
                    .ToListAsync();

                if (newValidAuthorIds == null || newValidAuthorIds.Count == 0)
                {
                    return BadRequest("Nem lett érvényes szerző megadva!");
                }

                // Új szerző hozzáadása

                foreach (var aid in newValidAuthorIds)
                {
                    var existingRelation = book.BookAuthors
                                            .FirstOrDefault(ba => ba.AuthorId == aid);

                    if (existingRelation == null)
                    {
                        book.BookAuthors.Add(new BookAuthor
                        {
                            BookId = book.BookId,
                            AuthorId = aid,
                            Created = now,
                            Modified = now,
                            Active = true
                        });

                    }
                    else if (!existingRelation.Active)
                    {
                        // Ha korábban inaktív volt, akkor újraaktiváljuk
                        existingRelation.Active = true;
                        existingRelation.Modified = now;
                    }
                }

                // Szerző törlése (IsActive=false)

                foreach (var aid in existingAuthorIds)
                {
                    if (!bookDto.AuthorIds.Contains(aid))
                    {
                        var bookAuthor = book.BookAuthors.FirstOrDefault(ba => ba.AuthorId == aid);

                        if (bookAuthor != null)
                        {
                            bookAuthor.Active = false;
                            bookAuthor.Modified = now;
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {

            // Elérhető példányok kezdeti értéke megegyezik a teljes példányszámmal

            var now = DateTime.Now;

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
                Created = now,
                Modified = now,
                Active = true
            };

            // Szerző és műfaj kapcsolatok létrehozása

            // Műfajok hozzáadása

            if (bookDto.GenreIds != null && bookDto.GenreIds.Count > 0)
            {
                var genres = await _context.Genres
                        .Where(g => bookDto.GenreIds.Contains(g.GenreId) && g.Active)
                        .ToListAsync();

                if (genres == null || genres.Count == 0)
                {
                    return BadRequest("Nem lett érvényes műfaj megadva!");
                }

                foreach (var genre in genres)
                {
                    book.BookGenres.Add(
                        new BookGenre
                        {
                            Genre = genre,
                            Created = now,
                            Modified = now,
                            Active = true
                        }
                     );
                }
            }

            // Szerzők hozzáadása

            if (bookDto.AuthorIds != null && bookDto.AuthorIds.Count > 0)
            {
                var authors = await _context.Authors
                        .Where(a => bookDto.AuthorIds.Contains(a.AuthorId) && a.Active)
                        .ToListAsync();

                if (authors == null || authors.Count == 0)
                {
                    return BadRequest("Nem lett érvényes szerző megadva!");
                }

                foreach (var author in authors)
                {
                    book.BookAuthors.Add(
                        new BookAuthor
                        {
                            Author = author,
                            Created = now,
                            Modified = now,
                            Active = true
                        }
                     );
                }
            }

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            var createdBookDto = MapToDto(book);

            return CreatedAtAction("GetBook", new { id = book.BookId }, createdBookDto);
        }
        /*
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
        */

        // GET: api/Books/inStock
        [HttpGet("inStock")]
        public async Task<ActionResult<IEnumerable<BookReadDto>>> GetAvailableBooks()
        {
            var books = await _context.Books
                // Betöltjük a kapcsolatokat (Szerzők, Műfajok) a megjelenítéshez
                .Include(b => b.BookAuthors.Where(ba => ba.Active && ba.Author.Active))
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres.Where(b => b.Active && b.Genre.Active))
                .ThenInclude(bg => bg.Genre)
                // Szűrés: Csak aktív könyvek, amikből van készleten (CurrentInventoryCount > 0)
                .Where(b => b.Active && b.CurrentInventoryCount > 0) 
                .ToListAsync();

            return books.Select(b => MapToDto(b)).ToList();
        }

        // GET: api/Books/top/{count}
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<BookTopBorrowDto>>> GetTopBooks(int count)
        {
            if (count <= 0)
            {
                return BadRequest("A darabszámnak pozitív egész számnak kell lennie.");
            }

            var topBooks = await _context.Books
                .Select(b => new BookTopBorrowDto
                {
                    BookId = b.BookId,
                    HungarianTitle = b.HungarianTitle,
                    OriginalTitle = b.OriginalTitle,
                    Publisher = b.Publisher,
                    PublishedYear = b.PublishedYear,
                    TotalBorrows = b.Borrows.Count, // Count the related Borrow entities
                    Active = b.Active
                })
                .OrderByDescending(b => b.TotalBorrows) // Order by the calculated count
                .Take(count)
                .ToListAsync();

            return Ok(topBooks);
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        private BookReadDto MapToDto(Book book)
        {
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
                Genres = book.BookGenres.Select(bg => new GenreForStatDto
                {
                    GenreId = bg.Genre.GenreId,
                    Name = bg.Genre.GenreName
                }).ToList(),
                Authors = book.BookAuthors.Select(ba => new AuthorForStatDto
                {
                    AuthorId = ba.Author.AuthorId,
                    Name = ba.Author.AuthorName
                }).ToList(),

                
                Active = book.Active,
                Created = book.Created,
                Modified = book.Modified,
            };
        }
    }
}
