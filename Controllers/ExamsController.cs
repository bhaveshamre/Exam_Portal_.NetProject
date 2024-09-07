using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Data;
using OnlineExam.Models;

namespace OnlineExam.Controllers
{
    public class ExamsController : Controller
    {
        private readonly OnlineExamContext _context;

        public ExamsController(OnlineExamContext context)
        {
            _context = context;
        }

        // GET: Exams
        public async Task<IActionResult> Index()
        {
            

            var availableExams = await _context.Exams

                .ToListAsync();
            ViewBag.msg = "Admin";

            return View(availableExams);
        }

        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: Exams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamId,Title,Description,StartTime,EndTime,CreatedBy")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamId,Title,Description,StartTime,EndTime,CreatedBy")] Exam exam)
        {
            if (id != exam.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.ExamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.ExamId == id);
        }
        public async Task<IActionResult> AddQuestions(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(new Question
            {
                ExamId = id
            });
        }

        // POST: Exam/AddQuestions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestions(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = question.ExamId });
            }

            return View(question);
        }

        // GET: Exam/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);


        }

        public async Task<IActionResult> StudentExams()
        {
            // Assuming that student's ID is stored in session after login
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                // Redirect to login page if student ID is not available
                return RedirectToAction("Login", "Students");
            }

            var exams = await _context.Exams
                .Where(e => !e.Results.Any(r => r.StudentId == studentId))
                .ToListAsync();

            return View(exams);
        }

        // GET: Exams/StartExam/5
        public async Task<IActionResult> StartExam(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
        [HttpPost]
        public IActionResult SubmitExam(int examId)
        {
            // Check if the student is logged in
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Students");
            }

            // Use TempData to pass a success message to the view
            TempData["SuccessMessage"] = "Exam submitted successfully!";

            // Redirect back to the exam page or wherever appropriate
            return RedirectToAction("StartExam", new { id = examId });

        }

    }

}