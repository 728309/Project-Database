using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private static List<Room> rooms = new List<Room>
    {
        new Room(1, "Dormitory", 8, 101),
        new Room(2, "Single", 1, 102),
        new Room(3, "Dormitory", 6, 103),
        new Room(4, "Single", 1, 104),
        new Room(5, "Dormitory", 8, 105),
        new Room(6, "Single", 1, 106)
    };

        public List<Room> GetAllRooms()
        {
            return rooms ?? new List<Room>();
        }

        public Room GetRoomById(int id)
        {
            return rooms.FirstOrDefault(r => r.RoomID == id);
        }

        public void AddRoom(Room room)//check out
        {
            room.RoomID = rooms.Max(r => r.RoomID) + 1;
            rooms.Add(room);
        }

        public void UpdateRoom(Room updatedRoom)//check out
        {
            var room = rooms.FirstOrDefault(r => r.RoomID == updatedRoom.RoomID);
            if (room != null)
            {
                room.RoomType = updatedRoom.RoomType;
                room.Capacity = updatedRoom.Capacity;
                room.RoomNumber = updatedRoom.RoomNumber;
            }

        }
        public void DeleteRoom(int id)//checkout
        {
            var room = rooms.FirstOrDefault(r => r.RoomID == id);
            if (room != null)
            {
                rooms.Remove(room);
            }
        }

    }


}


