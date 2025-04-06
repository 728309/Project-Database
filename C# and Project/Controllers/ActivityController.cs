using C__and_Project.Models;
using C__and_Project.Repositories;
using C__and_Project.ViewModels;
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
            var allLecturers = _lecturerRepository.GetAllLecturers();
            var supervisors = _lecturerRepository.GetSupervisorsByActivityId(activityId);

            var availableLecturers = allLecturers
                .Where(l => !supervisors.Any(s => s.LecturerID == l.LecturerID))
                .ToList();

            var viewModel = new ManageSupervisorsViewModel
            {
                ActivityId = activityId,
                Supervisors = supervisors,
                AvailableSupervisors = availableLecturers
            };

            return View(viewModel);
        }



        // Add Supervisor to an Activity
        public IActionResult AddSupervisor(int activityId, int lecturerId)
        {
            _supervisorRepository.AddSupervisorToActivity(activityId, lecturerId);
            TempData["Message"] = "Supervisor successfully added.";
            return RedirectToAction("ManageSupervisors", new { activityId });
        }

        // Remove Supervisor from an Activity
        public IActionResult RemoveSupervisor(int activityId, int lecturerId)
        {
            _supervisorRepository.RemoveSupervisorFromActivity(activityId, lecturerId);
            TempData["Message"] = "Supervisor successfully removed.";
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
        public IActionResult AddParticipant(int activityId, int studentId)
        {
            _activityRepository.AddParticipantToActivity(activityId, studentId);
            TempData["Message"] = "Participant successfully added.";
            return RedirectToAction("ManageParticipants", new { activityId });
        }

        public IActionResult RemoveParticipant(int activityId, int studentId)
        {
            _activityRepository.RemoveParticipantFromActivity(activityId, studentId);
            TempData["Message"] = "Participant successfully removed.";
            return RedirectToAction("ManageParticipants", new { activityId });
        }
    }
}
