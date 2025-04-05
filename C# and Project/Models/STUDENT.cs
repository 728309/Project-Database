using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace C__and_Project.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required]
        public int StudentNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Room { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public Student()
        {

        }

        public Student(int studentID,int studentnumber, int room, DateTime date,
                    string firstname, string lastname)
        {
            StudentID = studentID;
            StudentNumber = studentnumber;
            Room = room;
            Date = date;
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
