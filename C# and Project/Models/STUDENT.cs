using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public int Room { get; set; }
        public int StudentNmbr { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Student()
        {

        }

        public Student(int studentID,int studentnmbr, int room, DateTime date,
                    string firstname, string lastname)
        {
            StudentID = studentID;
            StudentNmbr = studentnmbr;
            Room = room;
            Date = date;
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
