using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private static List<Room> rooms = new List<Room>
    {
       new Room(1, "Dormitory", 30, 101),
        new Room(2, "Single", 25, 102),
        new Room(3, "Dormitory", 40, 103),
        new Room(4, "Single", 20, 104),
        new Room(5, "Dormitory", 35, 105),
        new Room(6, "Single", 50, 106)
    };

        public List<Room> GetAllRooms()
        {
            return rooms ?? new List<Room>();
        }

        public Room GetRoomById(int id)
        {
            return rooms.FirstOrDefault(r => r.RoomID == id);
        }

        public void AddRoom(Room room)
        {
            rooms.Add(room);
        }
    }
}
