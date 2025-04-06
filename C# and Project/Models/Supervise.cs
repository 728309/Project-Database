namespace C__and_Project.Models
{
    public class Supervise
    {
        public int ActivityID { get; set; }
        public int LecturerID { get; set; }

        public Supervise() { }

        public Supervise(int activityID, int lecturerID)
        {
            ActivityID = activityID;
            LecturerID = lecturerID;
        }

    }
}
