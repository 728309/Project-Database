using C__and_Project.Models;
using C__and_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILecturerRepository _lecturerRepository;

        public LecturerController(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }
        public IActionResult Index(string lastNameFilter)
        {
            List<Lecturer> lecturers = _lecturerRepository.GetAllLecturers();

            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                lecturers = lecturers.Where(l => l.LastName.StartsWith(lastNameFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(lecturers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Lecturer lecturer)
        {

            if (!ModelState.IsValid)
            {
                return View(lecturer);
            }

            try
            { 
                _lecturerRepository.AddLecturer(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(lecturer);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            else
            {
                Lecturer? lecturer = _lecturerRepository.GetLecturerByID((int) id);
                return View(lecturer);
            }
        }
        [HttpPost]
        public IActionResult Delete(Lecturer lecturer)
        {
            try
            {
                _lecturerRepository.DeleteLecturer(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(lecturer);
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
                Lecturer? lecturer = _lecturerRepository.GetLecturerByID((int)id);
                if (lecturer == null)
                {
                    return NotFound();
                }

                return View(lecturer);
            }
        }

        [HttpPost]
        public IActionResult Edit(Lecturer lecturer)
        {

            if (!ModelState.IsValid)
            {
                return View(lecturer);
            }

            try
            {
                
                _lecturerRepository.UpdateLecturer(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating lecturer: {ex.Message}");
                return View(lecturer);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
