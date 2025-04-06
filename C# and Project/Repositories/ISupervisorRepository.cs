using System.Collections.Generic;
using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public interface ISupervisorRepository
    {
        void AddSupervisorToActivity(int activityId, int lecturerId);
        void RemoveSupervisorFromActivity(int activityId, int lecturerId);
        List<Lecturer> GetAvailableLecturersForActivity(int activityId); // Not currently supervising this activity
    }
}
