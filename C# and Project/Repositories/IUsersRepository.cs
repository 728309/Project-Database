using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class IUsersRepository
    {
        List<User> GetAllUsers();
        User? GetUserByID(int userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
