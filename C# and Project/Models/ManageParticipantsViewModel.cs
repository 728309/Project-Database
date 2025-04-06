namespace C__and_Project.Models
{
    public class ManageParticipantsViewModel
    {
        public Activity Activity { get; set; } // The selected activity
        public List<Student> Participants { get; set; } // List of students participating in the activity
        public List<Student> AvailableStudents { get; set; } // List of students who are not participating in the activity

    }
}
