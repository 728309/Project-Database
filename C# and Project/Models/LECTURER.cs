using System.Numerics;

namespace C__and_Project.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string DateofBirth { get; set; }
        //I feel like this should be DateTime but I'm not sure how to get it to work exactly yet
        public Lecturer()
        {

        }

        public Lecturer(int lecturerID,
                    string firstName, string lastName, int phoneNumber, string dateofBirth)
        {
            LecturerID = lecturerID;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DateofBirth = dateofBirth;
        }
    }
}
