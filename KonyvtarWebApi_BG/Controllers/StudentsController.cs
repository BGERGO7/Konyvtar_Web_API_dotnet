using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonyvtarWebApi_BG.Models;
using KonyvtarWebApi_BG.DTOs.Student;

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
                    LibraryCardId = x.LibraryCard.LibraryCardId,
                    CardIssueDate = x.LibraryCard.IssueDate,
                    CardExpirationDate = x.LibraryCard.ExpiryDate
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
                LibraryCardId = student.LibraryCard.LibraryCardId,
                CardIssueDate = student.LibraryCard.IssueDate,
                CardExpirationDate = student.LibraryCard.ExpiryDate
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
                LibraryCardId=student.LibraryCard.LibraryCardId,
                IssueDate = student.LibraryCard.IssueDate,
                ExpiryDate = student.LibraryCard.ExpiryDate,
                LibrayCardActive = student.LibraryCard.Active,
                LibraryCardCreated = student.LibraryCard.Created,
                LibraryCardModified = student.LibraryCard.Modified,
            };
        }


       /*
        // GET: api/students/{id}/borrows
        [HttpGet("{id}/borrows")]
        public async Task<ActionResult<StudentGetBorrows>> GetStudentBorrows(int id)
        {
            var student = await _context.Students
                .Include(x => x.Borrows)
                .FirstOrDefaultAsync(y => y.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return new StudentGetBorrows
            {
                StudentId = student.StudentId,
                BookNames = student.Borrows
            };
        }
       */

        // PUT: api/students/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStudentStatus(int id, StudentChangeStatus studentDto)
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
            student.StudentName = studentDto.StudentName;
            student.PlaceOfBirth = studentDto.PlaceOfBirth;
            student.DateOfBirth = studentDto.DateOfBirth;
            student.Address = studentDto.Address;
            student.Class = studentDto.Class;
            student.EmailAddress = studentDto.EmailAddress;
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentCreateDto studentDto)
        {
            var student = new Student
            {
                StudentName = studentDto.StudentName,
                PlaceOfBirth = studentDto.PlaceOfBirth,
                DateOfBirth = studentDto.DateOfBirth,
                Address = studentDto.Address,
                Class = studentDto.Class,
                EmailAddress = studentDto.EmailAddress,
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
