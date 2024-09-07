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
    public class StudentsController : Controller
    {
        private readonly OnlineExamContext _context;

        public StudentsController(OnlineExamContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }
        public IActionResult Login()
        {
            return View();
        }

        // POST: Students/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Email == model.Email && s.PasswordHash == model.Password);

                if (student != null)
                {
                    // Set session
                    HttpContext.Session.SetInt32("StudentId", student.StudentId);
                    HttpContext.Session.SetString("StudentName", student.FullName);
                    HttpContext.Session.SetString("Role", "Student");
                   var availableExams = await _context.Exams
           
                 .ToListAsync();

                    if (availableExams.Any())
                    {
                        ViewBag.msg = "Student";

                        return RedirectToAction("Index", "Exams");
                    }
                    else
                    {
                        // Redirect to a page informing the student that no exams are available
                        return RedirectToAction("NoExams", "Home");
                    }// or redirect to another action
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.Students.Find(studentId);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(Student model)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student != null)
                {
                    student.FullName = model.FullName;
                    student.Email = model.Email;
                    student.EnrollmentDate = model.EnrollmentDate;
                    student.Course = model.Course;

                    _context.Update(student);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }


    private const string AdminEmail = "admin@gmail.com"; // Replace with your static admin email
        private const string AdminPassword = "AdminPassword123";

        public IActionResult Admin()
        {
            return View();
        }

        // POST: Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Admin(AdminLogin model)
        {
            if (ModelState.IsValid)
            {
                // Check if the credentials match the static values
                if (model.Email == AdminEmail && model.Password == AdminPassword)
                {
                    // Store admin login status in TempData
                    TempData["IsAdmin"] = true;
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        // POST: Admin/Logout
        [HttpPost]
        public IActionResult ALogout()
        {
            // Clear admin login status
            TempData["IsAdmin"] = null;
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var studentId = HttpContext.Session.GetInt32("StudentId");
                if (studentId == null)
                {
                    return RedirectToAction("Login");
                }

                var student = await _context.Students.FindAsync(studentId);
                if (student == null)
                {
                    return NotFound();
                }

                if (student.PasswordHash != model.OldPassword)
                {
                    ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                    return View(model);
                }

                student.PasswordHash = model.NewPassword;
                _context.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile");
            }

            return View(model);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FullName,Email,PasswordHash,EnrollmentDate,Course")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FullName,Email,PasswordHash,EnrollmentDate,Course")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
