using System;
using System.Collections.Generic;
using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> GetAllActivities();
        Activity GetActivityById(int id);
        void AddActivity(Activity activity);
        void UpdateActivity(Activity activity);
        void DeleteActivity(int id);
        bool ActivityExists(string name);

        //check
        List<Student> GetParticipantsByActivityId(int activityId); // Get students for a specific activity
        List<Student> GetAvailableStudents(int activityId); // Get students not participating in a specific activity
        void AddStudentToActivity(int activityId, int studentId); // Add a student to the activity
        void RemoveStudentFromActivity(int activityId, int studentId); // Remove a student from the activity



    }
}
