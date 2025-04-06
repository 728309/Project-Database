using C__and_Project.Models;

namespace C__and_Project.Views.Activity
{
    public class ManageParticipants
    {
        public Activity Activity { get; set; }
        public List<Student> Participants { get; set; }
        public List<Student> AvailableStudents { get; set; }
    }
}
