using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Genre;
using KonyvtarWebApi_BG.DTOs.Book;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly LibraryContext _context;

        public GenresController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreReadDto>>> GetGenres()
        {
            return await _context.Genres
            .Where(x => x.Active) // Soft delete szűrés
            .Select(x => new GenreReadDto
            {
                GenreId = x.GenreId,
                GenreName = x.GenreName,
                Active = x.Active,
                Created = x.Created,
                Modified = x.Modified
            })
            .ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreReadDto>> GetGenre(int id)
        {
            // FindAsync helyett query Active szűréssel
            var genre = await _context.Genres
                .Where(g => g.Active)
                .FirstOrDefaultAsync(g => g.GenreId == id);

            if (genre == null)
            {
                return NotFound();
            }

            return new GenreReadDto
            {
                GenreId = genre.GenreId,
                GenreName = genre.GenreName,
                Active = genre.Active,
                Created = genre.Created,
                Modified = genre.Modified
            };
        }

        // GET: api/Genres/{id}/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookByGenreDto>>> GetBooksByGenre(int id)
        {
            // Csak aktív műfajnál adunk vissza könyveket
            var genreExists = await _context.Genres.AnyAsync(g => g.GenreId == id && g.Active);

            if (!genreExists)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Where(b => b.Active && b.BookGenres!.Any(bg => bg.GenreId == id)) // Könyv is legyen aktív
                .Select(b => new BookByGenreDto 
                {
                    BookId = b.BookId,
                    HungarianTitle = b.HungarianTitle,
                    OriginalTitle = b.OriginalTitle,
                    AuthorId = b.BookAuthors!.Select(ba => ba.AuthorId).ToList(),
                    AuthorName = b.BookAuthors!.Select(ba => ba.Author!.AuthorName).ToList(),
                    Genres = b.BookGenres!.Select(bg => bg.Genre!.GenreName ?? string.Empty).ToList(),
                    CurrentlyAvailable = b.CurrentInventoryCount,
                    ReleaseYear = b.PublishedYear,
                    Active = b.Active,
                    Created = b.Created,
                    Modified = b.Modified
                })
                .ToListAsync();

            return Ok(books);
        }  

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreUpdateDto genreDto)
        {

            if (id != genreDto.GenreId)
            {
                return BadRequest();
            }

            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            genre.GenreName = genreDto.GenreName!;
            genre.Active = genreDto.Active;
            genre.Modified = DateTime.UtcNow;
            //genre.Created = genre.Created;

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(GenreCreateDto genreDto)
        {
            var genre = new Genre
            {
                GenreName = genreDto.GenreName!,
                Active = true,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
        
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            var createdGenre = MapToDto(genre); 

            return CreatedAtAction("GetGenre", new { id = genre.GenreId }, createdGenre);
        }
        /*
        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.GenreId == id);
        }

        private GenreReadDto MapToDto(Genre genre)
        {
            return new GenreReadDto
            {
                GenreId = genre.GenreId,
                GenreName = genre.GenreName,
                Active = genre.Active,
                Created = genre.Created,
                Modified = genre.Modified
            };
        }
    }
}
