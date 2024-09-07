using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// Models/Student.cs

namespace OnlineExam.Models
{

   // [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name must only contain letters and spaces.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string  PasswordHash { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime EnrollmentDate { get; set; }

        //[StringLength(100, ErrorMessage = "Course name cannot be longer than 100 characters.")]
        //[RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Course name must only contain letters and spaces.")]
        public string Course { get; set; }
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
    }
}