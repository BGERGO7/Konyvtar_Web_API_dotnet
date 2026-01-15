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
            return await _context.Authors
                .Select(a => new AuthorReadDto
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    DateOfBirth = a.DateOfBirth,
                    PlaceOfBirth = a.PlaceOfBirth,
                    Biography = a.Biography,
                    Active = a.Active,
                    Created = a.Created,
                    Modified = a.Modified
                })  
                .ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

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

        // GET: api/Authors/{id}/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<AuthorGetBooksDto>> GetAuthorBooks(int id)
        {
            var author = await _context.Authors
                .Include(x => x.BookAuthors)
                .ThenInclude(b => b.Book)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            var books = author.BookAuthors
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
                Books = books,
            };
        }

        // PUT: api/Authors/5/changeStatus
        [HttpPut("{id}/changeStatus")]
        public async Task<IActionResult> UpdateAuthorStatus(int id, AuthorStatusDto authorDto)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }
            author.Modified = DateTime.UtcNow;
            author.Active = authorDto.Active;
            author.Created = authorDto.Created;

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
                    StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.AuthorId)
            {
                return BadRequest();
            }

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


            //_context.Entry(author).State = EntityState.Modified;

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
                    StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDto authorDto)
        {
            var author = new Author
            {
                AuthorName = authorDto.AuthorName,
                DateOfBirth = authorDto.DateOfBirth,
                PlaceOfBirth = authorDto.PlaceOfBirth,
                Biography = authorDto.Biography,
                Active = authorDto.Active,
                Created = DateTime.UtcNow,
                Modified = authorDto.Modified
            };  
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}
