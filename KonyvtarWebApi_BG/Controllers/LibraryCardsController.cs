using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.LibraryCard;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LibraryCardsController(LibraryContext context)
        {
            _context = context;
        }
        
        // GET: api/LibraryCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryCardReadDto>>> GetLibraryCards()
        {
            var libraryCards = await _context.LibraryCards
                .Where(l => l.Active) // Csak aktív kártyák
                .ToListAsync();

            return libraryCards.Select(l => MapToDto(l)).ToList();
        }

        // GET: api/LibraryCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryCardReadDto>> GetLibraryCard(int id)
        {
            // FindAsync helyett szűrés
            var libraryCard = await _context.LibraryCards
                .Where(l => l.Active)
                .FirstOrDefaultAsync(l => l.LibraryCardId == id);

            if (libraryCard == null)
            {
                return NotFound();
            }

            return MapToDto(libraryCard);
        }
        
        // PUT: api/LibraryCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibraryCard(int id, LibraryCardUpdateDto libraryCardDto)
        {
            if (id != libraryCardDto.LibraryCardId)
            {
                return BadRequest();
            }

            if (libraryCardDto.IssueDate >= libraryCardDto.ExpiryDate)
            {
                return BadRequest(new { message = "A kiadási dátum nem lehet későbbi vagy egyenlő a lejárati dátummal." });
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);

            if (libraryCard == null)
            {
                return NotFound();
            }

            // Ha változott a diák azonosítója, ellenőrizzük az új tulajdonost
            if (libraryCard.StudentId != libraryCardDto.StudentId)
            {
                var newStudent = await _context.Students
                    .Include(s => s.LibraryCard)
                    .FirstOrDefaultAsync(s => s.StudentId == libraryCardDto.StudentId);

                if (newStudent == null)
                {
                    return BadRequest(new { message = "A megadott új tulajdonos (diák) nem létezik." });
                }

                if (!newStudent.Active)
                {
                    return BadRequest(new { message = "A megadott új tulajdonos (diák) inaktív, így nem kaphat kártyát." });
                }

                // Ellenőrizzük, hogy az új diáknak van-e már MÁSIK kártyája
                if (newStudent.LibraryCard != null && newStudent.LibraryCard.LibraryCardId != id)
                {
                    return BadRequest(new { message = "A választott új tulajdonos már rendelkezik egy másik könyvtárkártyával." });
                }
                
                // Ha minden rendben, átírjuk a tulajdonost
                libraryCard.StudentId = libraryCardDto.StudentId;
            }

            // Többi adat frissítése
            libraryCard.IssueDate = libraryCardDto.IssueDate;
            libraryCard.ExpiryDate = libraryCardDto.ExpiryDate;
            libraryCard.Active = libraryCardDto.Active;
            libraryCard.Modified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryCardExists(id))
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
        

        // POST: api/LibraryCards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LibraryCardCreateDto>> PostLibraryCard(LibraryCardCreateDto libraryCardDto)
        {
            if(libraryCardDto.IssueDate >= libraryCardDto.ExpiryDate)
            {
                return BadRequest(new { message = "A kiadási dátum nem lehet későbbi vagy egyenlő a lejárati dátummal." });
            }

            var student = await _context.Students
                .Include(s => s.LibraryCard)
                .FirstOrDefaultAsync(s => s.StudentId == libraryCardDto.StudentId);

            var now = DateTime.UtcNow;
            if (student == null)
            {
                return NotFound(new { message = "A megadott diák nem létezik." });
            }

            if (!student.Active)
            {
                return BadRequest(new { message = "Inaktív diák nem kaphat könyvtárkártyát." });
            }

            if (student.LibraryCard != null)
            {
                return BadRequest(new { message = "A diák már rendelkezik könyvtárkártyával." });
            }

            var libraryCard = new LibraryCard
            {
                StudentId = libraryCardDto.StudentId,
                IssueDate = libraryCardDto.IssueDate,
                ExpiryDate = libraryCardDto.ExpiryDate,
                Active = true,
                Created = now,
                Modified = now
            };

            _context.LibraryCards.Add(libraryCard);
            await _context.SaveChangesAsync();

            var createdDto = MapToDto(libraryCard);

            return CreatedAtAction("GetLibraryCard", new { id = libraryCard.LibraryCardId }, createdDto);
        }       
        
        // DELETE: api/LibraryCards/5
        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibraryCard(int id)
        {
            var libraryCard = await _context.LibraryCards.FindAsync(id);
            if (libraryCard == null)
            {
                return NotFound();
            }

            _context.LibraryCards.Remove(libraryCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool LibraryCardExists(int id)
        {
            return _context.LibraryCards.Any(e => e.LibraryCardId == id);
        }

        private LibraryCardReadDto MapToDto(LibraryCard libraryCard)
        {
            return new LibraryCardReadDto
            {
                LibraryCardId = libraryCard.LibraryCardId,
                StudentId = libraryCard.StudentId,
                IssueDate = libraryCard.IssueDate,
                ExpiryDate = libraryCard.ExpiryDate,
                Active = libraryCard.Active,
                Created = libraryCard.Created,
                Modified = libraryCard.Modified
            };
        }
    }
}
