using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExam.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Exam ID is required.")]

        public int ExamId { get; set; }

        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(100, ErrorMessage = "Question text cannot be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Question text must only contain letters and spaces.")]
        public string QuestionText { get; set; } = null!;

        [Required(ErrorMessage = "Option A is required.")]
        [StringLength(100, ErrorMessage = "Option A cannot be longer than 100 characters.")]
        
        public string OptionA { get; set; } = null!;

        [Required(ErrorMessage = "Option B is required.")]
        [StringLength(100, ErrorMessage = "Option B cannot be longer than 100 characters.")]
        
        public string OptionB { get; set; } = null!;

        [Required(ErrorMessage = "Option C is required.")]
        [StringLength(100, ErrorMessage = "Option C cannot be longer than 100 characters.")]
       
        public string OptionC { get; set; } = null!;

        [Required(ErrorMessage = "Option D is required.")]
        [StringLength(100, ErrorMessage = "Option D cannot be longer than 100 characters.")]
        
        public string OptionD { get; set; } = null!;

        [Required(ErrorMessage = "Correct option is required.")]
        public string CorrectOption { get; set; } = null!;
        [ForeignKey("ExamId")]
        public virtual Exam? Exam { get; set; } = null!;


    }
}
