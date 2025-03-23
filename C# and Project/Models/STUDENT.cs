using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public int RoomID { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Student()
        {

        }

        public Student(int studentID, int roomID, DateTime date,
                    string firstname, string lastname)
        {
            StudentID = studentID;
            RoomID = roomID;
            Date = date;
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
