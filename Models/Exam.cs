using System.ComponentModel.DataAnnotations;

namespace OnlineExam.Models
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name must only contain letters and spaces.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime EndTime { get; set; }

        [StringLength(100, ErrorMessage = "Created by cannot be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name must only contain letters and spaces.")]
        [Required(ErrorMessage = "CreatedBy is required.")]
        public string? CreatedBy { get; set; }

        public virtual ICollection<Question>? Questions { get; set; } = new HashSet<Question>();

        public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    }
}
