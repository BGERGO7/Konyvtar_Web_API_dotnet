using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Stats;
using KonyvtarWebApi_BG.DTOs.Borrow;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BorrowsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Borrows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowReadDto>>> GetBorrows()
        {
            var loans = await _context.Borrows
                .Include(b => b.Student)
                .Include(b => b.Book)
                .ToListAsync();

            var loanDtos = loans.Select(b => MapToDto(b)).ToList();

            return loanDtos;
        }

        // GET: api/Borrows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowReadDto>> GetBorrow(int id)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Student)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.BorrowId == id);

            if (borrow == null)
            {
                return NotFound();
            }

            return MapToDto(borrow);
        }

        // POST: api/Borrows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BorrowReadDto>> PostBorrow(BorrowCreateDto borrowDto)
        {
            var now = DateTime.Now;

            var book = await _context.Books
                .Where(b => b.Active)
                .FirstOrDefaultAsync(b => b.BookId == borrowDto.BookId);

            if (book == null)
            {
                return NotFound("A megadott könyv nem található!");
            }

            // Van-e a könyvből elérhető példány

            if (book.CurrentInventoryCount <= 0)
            {
                return Conflict("Nincs elérhető példány a megadott könyvből!");
            }

            var student = await _context.Students
                .Include(s => s.LibraryCard)
                .Where(s => s.Active)
                .FirstOrDefaultAsync(s => s.StudentId == borrowDto.StudentId);

            if (student == null)
            {
                return NotFound("A megadott diák nem található!");
            }


            // A diák rendelkezik-e érvényes olvasójeggyel

            if (student.LibraryCard == null || student.LibraryCard.ExpiryDate < now)
            {
                return Forbid("A diák nem rendelkezik érvényes olvasójeggyel!");
            }

            var BorrowDate = borrowDto.BorrowDate ?? now;
            var dueDate = borrowDto.DueDate ?? BorrowDate.AddDays(book.MaxRentDays);

            if (borrowDto.DueDate <= BorrowDate)
            {
                return BadRequest("A visszahozatali dátum nem lehet korábbi vagy egyenlő a kölcsönzés dátumával!");
            }

            var borrow = new Borrow
            {
                StudentId = borrowDto.StudentId,
                BookId = borrowDto.BookId,
                BorrowDate = borrowDto.BorrowDate ?? now,
                DueDate = dueDate,
                Active = true,
                Created = now,
                Modified = now,
            };

            book.CurrentInventoryCount -= 1;
            book.Modified = now;
            _context.Borrows.Add(borrow);


            await _context.SaveChangesAsync();

            var createdBorrowDto = MapToDto(borrow);

            return CreatedAtAction("GetBorrow", new { id = borrow.BorrowId }, createdBorrowDto);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id, [FromBody] BorrowReturnDto borrowReturnDto)
        {
            if (id != borrowReturnDto.BorrowId)
            {
                return BadRequest("A kölcsönzés azonosítók nem egyeznek!");
            }

            var now = DateTime.Now;

            var loan = await _context.Borrows
                .Include(l => l.Book)
                .Where(l => l.Active)
                .FirstOrDefaultAsync(l => l.BorrowId == id);

            if (loan == null)
            {
                return NotFound("A megadott kölcsönzés nem található!");
            }

            if (loan.ReturnDate != null)
            {
                return BadRequest("A könyv már vissza lett hozva!");
            }

            loan.ReturnDate = now;
            loan.Modified = now;

            // Könyv példányszámának növelése
            loan.Book.CurrentInventoryCount += 1;
            loan.Book.Modified = now;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(loan));
        }

        private BorrowReadDto MapToDto(Borrow borrow)
        {
            return new BorrowReadDto
            {
                BorrowId = borrow.BorrowId,
                Student = new StudentForStatDto
                {
                    StudentId = borrow.Student.StudentId,
                    Name = borrow.Student.StudentName,
                },
                Book = new BookStatBaseDto
                {
                    BookId = borrow.Book.BookId,
                    HungarianTitle = borrow.Book.HungarianTitle,
                    OriginalTitle = borrow.Book.OriginalTitle
                },
                BorrowDate = borrow.BorrowDate,
                DueDate = borrow.DueDate,
                ReturnDate = borrow.ReturnDate
            };
        }
    }
}
