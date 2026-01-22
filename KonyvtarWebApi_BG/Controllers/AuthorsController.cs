using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Author;
using KonyvtarWebApi_BG.DTOs.Stats;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadDto>>> GetAuthors()
        {
           // Csak az aktív szerzők lekérdezése
           var authors = await _context.Authors
               .Where(a => a.Active)
               .ToListAsync(); 

             return authors.Select(a => MapToDto(a)).ToList();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
        {
            // FindAsync helyett FirstOrDefaultAsync, hogy szűrhessünk az Active mezőre
            var author = await _context.Authors
                .Where(a => a.Active)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }
            
            return MapToDto(author);
        }

        // GET: api/Authors/{id}/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<AuthorGetBooksDto>> GetAuthorBooks(int id)
        {
            var author = await _context.Authors
                .Include(x => x.BookAuthors)
                .ThenInclude(b => b.Book)
                .Where(a => a.Active) // Itt is szűrjük a szerzőt
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            var books = author.BookAuthors
                .Where(ba => ba.Book != null && ba.Book.Active) // Csak aktív könyveket mutassunk
                .Select(ba => ba.Book!)
                .Select(b => new BookWithInventoryDto
                {
                    BookId = b.BookId,
                    Title = b.OriginalTitle, 
                    CurrentInventory = b.CurrentInventoryCount,
                    Active = b.Active,
                    Created = b.Created,
                    Modified = b.Modified
                })
                .ToList();

            return new AuthorGetBooksDto
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                Books = books,
            };
        }

        // Többi metódus (PUT, POST) maradhat a régiben, 
        // de a PUT-nál érdemes figyelni, hogy Active státusztól függetlenül módosítható-e.
        // Általában módosításnál használhatjuk a FindAsync-ot közvetlenül, 
        // vagy ha szigorúak vagyunk, ott is szűrhetünk.

        // ... (PUT, POST implementációk változatlanok maradhatnak a korábbi kód alapján) ...
        
        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.AuthorId)
            {
                return BadRequest();
            }

            // Módosítani engedjük az inaktívat is (pl. visszaaktiválás miatt), 
            // ezért itt nem szűrünk Active-ra.
            var author = await _context.Authors.FindAsync(id); 

            if (author == null) {
                return NotFound();
            }

            author.AuthorName = authorDto.AuthorName;
            author.DateOfBirth = authorDto.DateOfBirth;
            author.PlaceOfBirth = authorDto.PlaceOfBirth;
            author.Biography = authorDto.Biography;
            author.Active = authorDto.Active;
            author.Modified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }
        
        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDto authorDto)
        {
            var now = DateTime.UtcNow;

            var author = new Author
            {
                AuthorName = authorDto.AuthorName,
                PlaceOfBirth = authorDto.PlaceOfBirth,
                DateOfBirth = authorDto.DateOfBirth,
                Biography = authorDto.Biography,
                Active = true,          
                Created = now,
                Modified = now
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var createdAuthorDto = MapToDto(author);

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, createdAuthorDto);
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }

        private AuthorReadDto MapToDto(Author author)
        {
            return new AuthorReadDto
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                DateOfBirth = author.DateOfBirth,
                PlaceOfBirth = author.PlaceOfBirth,
                Biography = author.Biography,
                Active = author.Active,
                Created = author.Created,
                Modified = author.Modified
            };
        }
    }
}
