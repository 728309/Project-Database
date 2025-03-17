using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private static List<Room> rooms = new List<Room>
    {
        new Room(1, "Single", 1, 101),
        new Room(2, "Dormitory", 6, 102)
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
