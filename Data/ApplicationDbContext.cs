using Microsoft.EntityFrameworkCore;
using OnlineExam.Models;

namespace OnlineExam.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
    }
}
