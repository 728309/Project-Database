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
        public IActionResult Index()
        {
            var lecturers = _lecturerRepository.GetAllLecturers();
            return View(lecturers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _lecturerRepository.AddLecturer(lecturer);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(lecturer);
        }
        public IActionResult Edit(int id)
        {
            var lecturer = _lecturerRepository.GetLecturerByID(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _lecturerRepository.UpdateLecturer(lecturer);
                return RedirectToAction("Index");
            }
            return View(lecturer);
        }
        public IActionResult Delete(int id)
        {
            var lecturer = _lecturerRepository.GetLecturerByID(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var lecturer = _lecturerRepository.GetLecturerByID(id);
            if (lecturer != null)
            {
                _lecturerRepository.DeleteLecturer(lecturer);
            }
            return RedirectToAction("Index");
        }
    }
}
