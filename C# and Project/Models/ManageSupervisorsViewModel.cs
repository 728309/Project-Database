using C__and_Project.Models;

namespace C__and_Project.ViewModels
{
    public class ManageSupervisorsViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public List<Lecturer> Supervisors { get; set; }
        public List<Lecturer> AvailableSupervisors { get; set; }
    }
}
