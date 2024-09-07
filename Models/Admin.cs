using System.ComponentModel.DataAnnotations;

namespace OnlineExam.Models
{
    public class Admin
    {

        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
