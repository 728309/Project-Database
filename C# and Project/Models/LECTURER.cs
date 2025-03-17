using System.Numerics;

namespace C__and_Project.Models
{
    public class LECTURER
    {
        public int LecturerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string DateofBirth { get; set; }

        public LECTURER()
        {

        }

        public LECTURER(int lecturerID,
                    string firstname, string lastname, int phoneNumber, string DoB)
        {
            LecturerID = lecturerID;
            FirstName = firstname;
            LastName = lastname;
            PhoneNumber = phoneNumber;
            DateofBirth = DoB;
        }
    }
}
