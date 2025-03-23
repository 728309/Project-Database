using C__and_Project.Models;
using C__and_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IActionResult Index(string lastNameFilter)
        {
            List<Student> students = _studentRepository.GetAllStudents();

            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                students = students.Where(s => s.LastName.StartsWith(lastNameFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(students);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                // Add student via repository
                _studentRepository.AddStudent(student);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(student);
            }
        }

        // Get student for deletion
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            else
            {
                // Get student via repository
                Student? student = _studentRepository.GetStudentByID((int)id);
                return View(student);
            }
        }

        [HttpPost]
        public IActionResult Delete(Student student)
        {
            try
            {
                // Delete student via repository
                _studentRepository.DeleteStudent(student);

                // Go back to student list (via Index)
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Something went wrong, return to view with student data
                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            else
            {
                // Get student via repository
                Student? student = _studentRepository.GetStudentByID((int)id);
                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
        }

            [HttpPost]
        public IActionResult Edit(Student student)
        {

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                // Update student via repository
                _studentRepository.UpdateStudent(student);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                return View(student);
            }
        }
    }
}
