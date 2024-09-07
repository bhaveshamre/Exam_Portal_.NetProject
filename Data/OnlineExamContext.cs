using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Models;

namespace OnlineExam.Data
{
    public class OnlineExamContext : DbContext
    {
        public OnlineExamContext (DbContextOptions<OnlineExamContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineExam.Models.Student> Students { get; set; } = default!;
        public DbSet<OnlineExam.Models.Exam> Exams { get; set; } = default!;
        public DbSet<OnlineExam.Models.Question> Questions { get; set; } = default!;
        public DbSet<OnlineExam.Models.Result> Results { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exams"); 

                entity.Property(e => e.ExamId)
                    .HasColumnName("exam_id"); 

                entity.Property(e => e.Title)
                    .HasColumnName("title"); 

                entity.Property(e => e.Description)
                    .HasColumnName("description"); 

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_time");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by"); 
            });
            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");

                entity.Property(q => q.QuestionId)
                    .HasColumnName("question_id");

                entity.Property(q => q.ExamId)
                    .HasColumnName("exam_id");

                entity.Property(q => q.QuestionText)
                    .HasColumnName("question_text");

                entity.Property(q => q.OptionA)
                    .HasColumnName("option_a");

                entity.Property(q => q.OptionB)
                    .HasColumnName("option_b");

                entity.Property(q => q.OptionC)
                    .HasColumnName("option_c");

                entity.Property(q => q.OptionD)
                    .HasColumnName("option_d");

                entity.Property(q => q.CorrectOption)
                    .HasColumnName("correct_option");

                // Optional: Configure the relationship
                entity.HasOne(q => q.Exam)
                    .WithMany(e => e.Questions)
                    .HasForeignKey(q => q.ExamId);
            });
             modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Results"); // Maps the entity to the Results table

                entity.Property(e => e.ResultId)
                    .HasColumnName("result_id"); // Maps ResultId property to result_id column

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id"); // Maps StudentId property to student_id column

                entity.Property(e => e.ExamId)
                    .HasColumnName("exam_id"); // Maps ExamId property to exam_id column

                entity.Property(e => e.Score)
                    .HasColumnName("score"); // Maps Score property to score column

                entity.Property(e => e.CompletedAt)
                    .HasColumnName("completed_at"); // Maps CompletedAt property to completed_at column

                // Define foreign key relationships
                entity.HasOne(r => r.Exam)
                    .WithMany(e => e.Results)
                    .HasForeignKey(r => r.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Student)
                    .WithMany(s => s.Results)
                    .HasForeignKey(r => r.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<AdminLogin>().HasNoKey(); // For keyless entities
            // For keyless entities
        }
        public DbSet<OnlineExam.Models.Login> Login { get; set; } = default!;
        public DbSet<OnlineExam.Models.AdminLogin> AdminLogin { get; set; } = default!;
    }
}
