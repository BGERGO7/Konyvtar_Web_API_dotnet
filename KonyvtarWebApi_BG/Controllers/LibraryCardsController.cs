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
        /*
        // GET: api/LibraryCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryCard>>> GetLibraryCards()
        {
            return await _context.LibraryCards.ToListAsync();
        }

        // GET: api/LibraryCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryCard>> GetLibraryCard(int id)
        {
            var libraryCard = await _context.LibraryCards.FindAsync(id);

            if (libraryCard == null)
            {
                return NotFound();
            }

            return libraryCard;
        }
        */


        // PUT: api/LibraryCards/5/changeExpiryDate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/changeExpiryDate")]
        public async Task<IActionResult> LibraryCardChangeDate(int id, LibraryCardChangeDate libraryCardDto)
        {
            if (id != libraryCardDto.LibraryCardId)
            {
                return BadRequest();
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);

            if (libraryCard == null)
            {
                return NotFound();
            }

            _context.Entry(libraryCard).State = EntityState.Modified;

           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!LibraryCardExists(id))
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



        // PUT: api/LibraryCards/5/activate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> LibraryCardActivate(int id, LibraryCardChangeStatus libraryCardDto)
        {
            if (id != libraryCardDto.LibraryCardId)
            {
                return BadRequest();
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);

            if (libraryCard == null)
            {
                return NotFound();
            }

            _context.Entry(libraryCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!LibraryCardExists(id))
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

        // POST: api/LibraryCards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LibraryCardCreateDto>> PostLibraryCard(LibraryCardCreateDto libraryCardDto)
        {
            var libraryCard = new LibraryCard
            {
                StudentId = libraryCardDto.StudentId,
                IssueDate = libraryCardDto.IssueDate,
                ExpiryDate = libraryCardDto.ExpiryDate,
                Active = true,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            _context.LibraryCards.Add(libraryCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibraryCard", new { id = libraryCard.LibraryCardId }, libraryCard);
        }

        // DELETE: api/LibraryCards/5
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

        private bool LibraryCardExists(int id)
        {
            return _context.LibraryCards.Any(e => e.LibraryCardId == id);
        }
    }
}
