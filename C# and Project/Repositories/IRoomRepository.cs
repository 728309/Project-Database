using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public interface IRoomRepository
    {
        List<Room> GetAllRooms();
        Room? GetRoomById(int id);
        void AddRoom(Room room);

        void UpdateRoom(Room room);

        void DeleteRoom(Room room);
    }

    //interfaces for repo

}
