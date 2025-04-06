using C__and_Project.Models;
using C__and_Project.Repositories;
using C__and_Project.ViewModels;
using C__and_Project.Views.Activity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace C__and_Project.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly ISupervisorRepository _supervisorRepository;

        public ActivityController(
            IActivityRepository activityRepository,
            ILecturerRepository lecturerRepository,
            ISupervisorRepository supervisorRepository)
        {
            _activityRepository = activityRepository;
            _lecturerRepository = lecturerRepository;
            _supervisorRepository = supervisorRepository;
        }


        // Index - Display all activities ordered by date and start time
        public IActionResult Index()
        {
            var activities = _activityRepository.GetAllActivities();
            return View(activities);
        }

        // Create - Display the form for adding a new activity
        public IActionResult Create()
        {
            var activity = new Activity();
            return View(activity);
        }

        // Create - Post action to add a new activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                if (_activityRepository.ActivityExists(activity.Name))
                {
                    ModelState.AddModelError("", "An activity with this name already exists.");
                    return View(activity);
                }

                _activityRepository.AddActivity(activity);
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // Edit - Display the form for editing an existing activity
        public IActionResult Edit(int id)
        {
            var activity = _activityRepository.GetActivityById(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // Edit - Post action to save the updated activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _activityRepository.UpdateActivity(activity);
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // Delete - Display the form for deleting an activity
        public IActionResult Delete(int id)
        {
            var activity = _activityRepository.GetActivityById(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // Delete - Post action to confirm and delete the activity
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _activityRepository.DeleteActivity(id);
            return RedirectToAction("Index");
        }

        public IActionResult ManageSupervisors(int activityId)
        {
            var activity = _activityRepository.GetActivityById(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            var allLecturers = _lecturerRepository.GetAllLecturers();
            var supervisors = _lecturerRepository.GetSupervisorsByActivityId(activityId);

            var availableLecturers = allLecturers
                .Where(l => !supervisors.Any(s => s.LecturerID == l.LecturerID))
                .ToList();

            var viewModel = new ManageSupervisorsViewModel
            {
                ActivityId = activityId,
                ActivityName = activity.Name, // ← Add this
                Supervisors = supervisors,
                AvailableSupervisors = availableLecturers
            };

            return View(viewModel);
        }





        public IActionResult AddSupervisor(int activityId, int lecturerId)
        {
            _supervisorRepository.AddSupervisorToActivity(activityId, lecturerId);

            var lecturer = _lecturerRepository.GetLecturerByID(lecturerId);
            if (lecturer != null)
            {
                TempData["Message"] = $"Successfully added supervisor: {lecturer.FirstName} (ID: {lecturer.LecturerID})";
            }
            else
            {
                TempData["Message"] = $"Successfully added supervisor with ID: {lecturerId}";
            }

            return RedirectToAction("ManageSupervisors", new { activityId });
        }


        public IActionResult RemoveSupervisor(int activityId, int lecturerId)
        {
            var lecturer = _lecturerRepository.GetLecturerByID(lecturerId);

            _supervisorRepository.RemoveSupervisorFromActivity(activityId, lecturerId);

            if (lecturer != null)
            {
                TempData["Message"] = $"Successfully removed supervisor: {lecturer.FirstName} (ID: {lecturer.LecturerID})";
            }
            else
            {
                TempData["Message"] = $"Successfully removed supervisor with ID: {lecturerId}";
            }

            return RedirectToAction("ManageSupervisors", new { activityId });
        }
        public IActionResult ManageParticipants(int activityId)
        {
            var participants = _activityRepository.GetParticipantsByActivityId(activityId);
            var nonParticipants = _activityRepository.GetAvailableStudents(activityId);
            var activity = _activityRepository.GetActivityById(activityId);
            var viewModel = new ManageParticipantsViewModel
            {
                Activity = activity,
                Participants = participants,
                AvailableStudents = nonParticipants
            };

            return View(viewModel);
        }
        public IActionResult AddSParticipant(int activityId, int studentId)
        {
            _activityRepository.AddStudentToActivity(activityId, studentId);
            TempData["Message"] = "Student successfully added to the activity.";
            return RedirectToAction("ManageParticipants", new { activityId });
        }


        public IActionResult RemoveStudent(int activityId, int studentId)
        {
            _activityRepository.RemoveStudentFromActivity(activityId, studentId);
            TempData["Message"] = "Student successfully removed from the activity.";
            return RedirectToAction("ManageParticipants", new { activityId });
        }


    }
}
