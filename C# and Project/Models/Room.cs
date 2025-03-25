namespace C__and_Project.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string TypeRoom { get; set; }
        public int Capacity { get; set; }

        public string RoomNumber { get; set; } //maybe change to string if you want to specify the building number

        public Room(int roomID, string typeRoom, int capacity, string roomNumber)
        {
            RoomID = roomID;
            TypeRoom = typeRoom;
            Capacity = capacity;
            RoomNumber = roomNumber;
        }
    }
}
