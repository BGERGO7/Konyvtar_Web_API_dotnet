using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Student;
using KonyvtarWebApi_BG.DTOs.Stats;

namespace KonyvtarWebApi_BG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public StudentsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetStudents()
        {
            // CSAK az aktív diákokat kérjük le
            var students = await _context.Students
                .Where(x => x.Active) 
                .ToListAsync();

            return students.Select(s => MapToDto(s)).ToList();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentReadDto>> GetStudent(int id)
        {
            var student = await _context.Students
                // Szűrés aktívra, így ha inaktív, null-t ad vissza -> NotFound lesz
                .Where(x => x.Active)
                .FirstOrDefaultAsync(y => y.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return MapToDto(student);
        }

        // GET: api/students/{id}/libraryCard
        [HttpGet("{id}/libraryCard")]
        public async Task<ActionResult<StudentGetLibraryCard>> GetStudentLibraryCard(int id)
        {
            // Itt is szűrjük a diákot aktívra
            var student = await _context.Students
                .Include(x => x.LibraryCard)
                // Opcionális: Ha az olvasójegynek is aktívnak kell lennie ahhoz, hogy lássuk,
                // akkor szűrhetnénk itt is, de lentebb egyszerűbb ellenőrizni.
                .Where(x => x.Active)
                .FirstOrDefaultAsync(y => y.StudentId == id);

            if (student == null)
            {
                return NotFound(); // Ha a diák inaktív vagy nem létezik
            }

            // Ha nincs olvasójegy, VAGY van, de "törölt" (inaktív)
            if (student.LibraryCard == null || !student.LibraryCard.Active)
            {
                return NotFound("A diáknak nincs aktív olvasójegye.");
            }

            return new StudentGetLibraryCard
            {
                StudentId = student.StudentId,
                LibraryCardId = student.LibraryCard.LibraryCardId,
                IssueDate = student.LibraryCard.IssueDate,
                ExpiryDate = student.LibraryCard.ExpiryDate,
                LibrayCardActive = student.LibraryCard.Active,
                LibraryCardCreated = student.LibraryCard.Created,
                LibraryCardModified = student.LibraryCard.Modified,
                Active = student.Active,
                Created = student.Created,
                Modified = student.Modified
            };
        }

        // GET: api/Students/top/{db}
        [HttpGet("top/{db}")]
        public async Task<ActionResult<IEnumerable<StudentTopBorrowDto>>> GetTopStudents(int db)
        {
           if(db <= 0)
            {
                return BadRequest("A darabszámnak pozitív egész számnak kell lennie.");
            }

            var topStudents = await _context.Students
                .Where(s => s.Active) // Csak aktív diákok kerülhetnek a toplistára
                .Select(s => new StudentTopBorrowDto
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Class = s.Class,
                    EmailAddress = s.EmailAddress,
                    // Feltételezzük, hogy a statisztikába a már inaktív (törölt) kölcsönzések nem számítanak bele
                    TotalBorrows = s.Borrows!.Count(b => b.Active), 
                    Active = s.Active,
                    Created = s.Created,
                    Modified = s.Modified
                })
                .OrderByDescending(s => s.TotalBorrows)
                .Take(db) 
                .ToListAsync();

            return Ok(topStudents);
        }

        // GET: api/students/{id}/borrows
        [HttpGet("{id}/borrows")]
        public async Task<ActionResult<StudentBorrowsStatsDto>> GetStudentBorrows(int id)
        {
            var student = await _context.Students
                // 1. Csak aktív Borrow rekordokat töltünk be
                .Include(s => s.Borrows!.Where(b => b.Active)) 
                .ThenInclude(b => b.Book!)
                .ThenInclude(bk => bk.BookAuthors!)
                .ThenInclude(ba => ba.Author)
                // 2. A diák maga is legyen aktív
                .Where(s => s.Active) 
                .FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                {
                    return NotFound();
                }
    
            var borrowDetails = student.Borrows!
                .OrderByDescending(b => b.BorrowDate) 
                .Select(b => new StudentBorrowDetailsDto
                {
                    BorrowId = b.BorrowId,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate,
                    BookId = b.BookId,
                    HungarianTitle = b.Book!.HungarianTitle,
                    OriginalTitle = b.Book.OriginalTitle,
                    Active = b.Book.Active,
                    Created = b.Book.Created,
                    Modified = b.Book.Modified,
                    AuthorNames = b.Book.BookAuthors!
                        .Select(ba => ba.Author!.AuthorName!)
                        .ToList()
                })
                .ToList();

            return new StudentBorrowsStatsDto
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                TotalBorrows = student.Borrows!.Count,
                ActiveBorrowsCount = student.Borrows.Count(b => b.ReturnDate == null), 
                Borrows = borrowDetails,
                Active = student.Active,
                Created = student.Created,
                Modified = student.Modified
            };
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentUpdateDto studentDto)
        {
            if (id != studentDto.StudentId)
            {
                return BadRequest();
            }

            // Itt az a kérdés, hogy inaktív diákot engedjünk-e szerkeszteni. 
            // Általában igen, vagy a szerkesztéssel egyben aktiválhatjuk is.
            // Ha szigorúan vesszük a törlést, akkor itt is kellene a szűrés, 
            // de adminisztrációs felületen hasznos lehet inaktívat is elérni.
            // A biztonság kedvéért itt hagyom FindAsync-kal (szűrés nélkül).
            var student = await _context.Students.FindAsync(id);
            
            if (student == null)
            {
                return NotFound();
            }

            student.StudentName = studentDto.StudentName!;
            student.PlaceOfBirth = studentDto.PlaceOfBirth!;
            student.DateOfBirth = studentDto.DateOfBirth;
            student.Address = studentDto.Address!;
            student.Class = studentDto.Class!;
            student.EmailAddress = studentDto.EmailAddress!;
            student.Active = studentDto.Active;
            
            student.Modified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentReadDto>> PostStudent(StudentCreateDto studentDto)
        {
            var now = DateTime.UtcNow;
            var student = new Student
            {
                StudentName = studentDto.StudentName!,
                PlaceOfBirth = studentDto.PlaceOfBirth!,
                DateOfBirth = studentDto.DateOfBirth,
                Address = studentDto.Address!,
                Class = studentDto.Class!,
                EmailAddress = studentDto.EmailAddress!,
                Active = studentDto.Active,
                Created = now,
                Modified = now
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetStudent", new { id = student.StudentId }, MapToDto(student));
        }
        
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }

        private static StudentReadDto MapToDto(Student student)
        {
            return new StudentReadDto
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                PlaceOfBirth = student.PlaceOfBirth,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                EmailAddress = student.EmailAddress,
                Active = student.Active,
                Created = student.Created,
                Modified = student.Modified
            };
        }
    }
}
