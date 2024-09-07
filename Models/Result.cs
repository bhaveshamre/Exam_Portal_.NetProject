using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExam.Models
{
    public class Result
    {

        [Key]
        public int ResultId { get; set; }

        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Exam ID is required.")]
        public int ExamId { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Score must be between 0.0 and 100.0")]
        public decimal? Score { get; set; }

        [Required(ErrorMessage = "Completed at is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime? CompletedAt { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam ? Exam  { get; set; } = null!;

        [ForeignKey("StudentId")]
        public virtual Student ? Student { get; set; } = null!;


    }
}
    