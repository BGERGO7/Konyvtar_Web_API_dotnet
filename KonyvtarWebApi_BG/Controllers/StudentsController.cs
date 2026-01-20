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
            return await _context.Students
                .Include(a => a.LibraryCard)
                .Select(x => new StudentReadDto
                {
                    StudentId = x.StudentId,
                    StudentName = x.StudentName,
                    PlaceOfBirth = x.PlaceOfBirth,
                    DateOfBirth = x.DateOfBirth,
                    Address = x.Address,
                    EmailAddress = x.EmailAddress,
                    Active = x.Active,
                    Created = x.Created,
                    Modified = x.Modified,
                    /*
                    LibraryCardId = x.LibraryCard!.LibraryCardId,
                    CardIssueDate = x.LibraryCard!.IssueDate,
                    CardExpirationDate = x.LibraryCard!.ExpiryDate
                    */
                })

                .ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentReadDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(x => x.LibraryCard)
                .FirstOrDefaultAsync(y => y.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

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
                Modified = student.Modified,
                /*
                LibraryCardId = student.LibraryCard!.LibraryCardId,
                CardIssueDate = student.LibraryCard.IssueDate,
                CardExpirationDate = student.LibraryCard.ExpiryDate
                */
             };
        }


        // GET: api/students/{id}/libraryCard
        [HttpGet("{id}/libraryCard")]
        public async Task<ActionResult<StudentGetLibraryCard>> GetStudentLibraryCard(int id)
        {
            var student = await _context.Students
                .Include(x => x.LibraryCard)
                .FirstOrDefaultAsync(y => y.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return new StudentGetLibraryCard
            {
                StudentId = student.StudentId,
                LibraryCardId=student.LibraryCard!.LibraryCardId,
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
        public async Task<ActionResult<StudentReadDto>> GetTopStudents(int db)
        {
            /*
             Diak azonosito
            Nev
            Osztaly
            Email cim

            Legaktivabb diakok a kolcsonzesek szama alapjan, megadott darabszamig ({id} -> Top5, Top10 stb)
             */

           if(db <= 0)
            {
                return BadRequest("A darabszámnak pozitív egész számnak kell lennie.");
            }

            var topStudents = await _context.Students
                .Select(s => new StudentTopBorrowDto
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Class = s.Class,
                    EmailAddress = s.EmailAddress,
                    TotalBorrows = s.Borrows!.Count,
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
            // 1. Eager Loading a Student -> Borrows -> Book -> BookAuthors -> Author láncra
            var student = await _context.Students
                .Include(s => s.Borrows!)
                .ThenInclude(b => b.Book!)
                .ThenInclude(bk => bk.BookAuthors!)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                {
                    return NotFound();
                }
    
            // 2. Projektálás (Mappelés) a StudentBorrowsStatsDto-ra
            // EF Core Projection (Select) használata az adatok alakítására.

            // A kölcsönzések listájának elkészítése:
            var borrowDetails = student.Borrows!
                // Rendezés (pl. legújabb kölcsönzések elöl)
                .OrderByDescending(b => b.BorrowDate) 
                .Select(b => new StudentBorrowDetailsDto
                {
                    BorrowId = b.BorrowId,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate,
                    
                    // Kiszámított tulajdonság: Lejárt-e (ha nincs visszahozva ÉS az esedékesség elmúlt)
                    //IsOverdue = b.ReturnDate == null && b.DueDate < DateTime.UtcNow, 

                    // Könyv adatok
                    BookId = b.BookId,
                    HungarianTitle = b.Book!.HungarianTitle,
                    OriginalTitle = b.Book.OriginalTitle,
                    Active = b.Book.Active,
                    Created = b.Book.Created,
                    Modified = b.Book.Modified,

                    // Szerző nevek kilapítása
                    AuthorNames = b.Book.BookAuthors!
                        .Select(ba => ba.Author!.AuthorName!)
                        .ToList()
                })
                .ToList();

            // Végleges Statisztikai DTO elkészítése
            return new StudentBorrowsStatsDto
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                TotalBorrows = student.Borrows!.Count,
                // Aktív kölcsönzés: Nincs ReturnDate beállítva
                ActiveBorrowsCount = student.Borrows.Count(b => b.ReturnDate == null), 
                
                Borrows = borrowDetails,
                Active = student.Active,
                Created = student.Created,
                Modified = student.Modified


            };
        }

        // PUT: api/students/{id}/changeStatus
        [HttpPut("{id}/changeStatus")]
        public async Task<IActionResult> ChangeStudentStatus(int id, StudentChangeStatus studentDto)
        {
            /*
            if (id != studentDto.StudentId)
            {
                return BadRequest();
            }
            */

            //_context.Entry(studentDto).State = EntityState.Modified;

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            student.StudentId = id;
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
                    StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }


       

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentUpdateDto studentDto)
        {
            if (id != studentDto.StudentId)
            {
                return BadRequest();
            }

            //_context.Entry(studentDto).State = EntityState.Modified;

            var student = _context.Students.Find(id);
            
            if (student == null)
            {
                return NotFound();
            }

            student.StudentId = id;
            student.StudentName = studentDto.StudentName!;
            student.PlaceOfBirth = studentDto.PlaceOfBirth!;
            student.DateOfBirth = studentDto.DateOfBirth;
            student.Address = studentDto.Address!;
            student.Class = studentDto.Class!;
            student.EmailAddress = studentDto.EmailAddress!;
            student.Active = studentDto.Active;
            student.Created = studentDto.Created;
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
                    StatusCode(500, new { message = "Adatbázis hiba történt", Error = ex.Message });
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentCreateDto studentDto)
        {
            var student = new Student
            {
                StudentName = studentDto.StudentName!,
                PlaceOfBirth = studentDto.PlaceOfBirth!,
                DateOfBirth = studentDto.DateOfBirth,
                Address = studentDto.Address!,
                Class = studentDto.Class!,
                EmailAddress = studentDto.EmailAddress!,
                Modified = studentDto.Modified,
                Created = DateTime.UtcNow,
                Active = true
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
