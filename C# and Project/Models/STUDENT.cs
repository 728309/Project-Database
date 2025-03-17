namespace C__and_Project.Models
{
    public class STUDENT
    {
        public int StudentID { get; set; }
        public int RoomID { get; set; }
        public string DateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public STUDENT()
        {

        }

        public STUDENT(int studentID, int roomID, string dateTime,
                    string firstname, string lastname)
        {
            StudentID = studentID;
            RoomID = roomID;
            DateTime = dateTime;
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
