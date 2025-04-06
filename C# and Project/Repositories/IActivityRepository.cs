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
        List<Student> GetParticipantsByActivityId(int activityId);
        List<Student> GetAvailableStudents(int activityId);
        void AddParticipantToActivity(int activityId, int studentId);
        void RemoveParticipantFromActivity(int activityId, int studentId);

    }
}
