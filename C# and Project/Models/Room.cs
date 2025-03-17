namespace C__and_Project.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }

        public int RoomNumber { get; set; } //maybe change to string if you want to specify the building number

        public Room(int roomID, string roomType, int capacity, int roomNumber)
        {
            RoomID = roomID;
            RoomType = roomType;
            Capacity = capacity;
            RoomNumber = roomNumber;
        }
    }
}
