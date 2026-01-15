using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentChangeStatus
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}
