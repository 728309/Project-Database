using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace C__and_Project.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }

        [Required]
        public int LecturerNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Room { get; set; }   
        public int PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }
        public Lecturer()
        {

        }
        public Lecturer(int lecturerID, int lecturerNumber,
                    string firstName, string lastName, int room, int phoneNumber, DateTime dateofBirth)
        {
            LecturerID = lecturerID;
            LecturerNumber = lecturerNumber;
            FirstName = firstName;
            LastName = lastName;
            Room = room;
            PhoneNumber = phoneNumber;
            DateofBirth = dateofBirth;
        }
    }
}
