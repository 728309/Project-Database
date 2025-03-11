using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class DummyUserRepository : IUsersRepository
    {
        //static to enable persisting of list
        //Ideally this should be done in a SEEDMETHOD!
        private static List<User> users =
                [
                    new User(1, "Peter Sauber", "06-123456", "pete@sauber@gmail.com"),
                    new User(2, "Bill Hodges", "06-44556677", "bill@hodges@gmail.com"),
                    new User(3, "Morris Belamy", "06-11228899", "morris@belamy@gmail.com")
                ];

        void IUsersRepository.AddUser(User user)
        {
            users.Add(user);
        }

        void IUsersRepository.DeleteUser(User user)
        {
            //just make sure the user is in the dataset
            User UserToRemove = users.SingleOrDefault(r => r.UserID == user.UserID);
            users.Remove(UserToRemove);
        }

        List<User> IUsersRepository.GetAllUsers()
        {
            return users;
        }

        User? IUsersRepository.GetUserByID(int userId)
        {
            return users.FirstOrDefault(x => x.UserID == userId);
        }

        void IUsersRepository.UpdateUser(User user)
        {
            var existingUser = users.FirstOrDefault(u => u.UserID == user.UserID);
            if (existingUser is not null)
            {
                existingUser.UserName = user.UserName;
                existingUser.MobileNumber = user.MobileNumber;
                existingUser.EmailAddress = user.EmailAddress;
            }
        }
    }
}
