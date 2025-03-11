using Microsoft.Data.SqlClients;
using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDBConnection");
        }

        List<User> IUsersRepository.GetAllUsers()
        {
            List<User> users = new List<User>();

            //below code must be re-factored to a seperate private method
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "Select UserID, UserName,  MobileNumber, EmailAddress from Users";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    command.Connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                UserName = reader["UserName"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString()
                            });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Something went wrong with the database", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Something went wrong reading data", ex);
                }
                //end method refactoring
            }
            return users;
        }

        User? IUsersRepository.GetUserByID(int userId)
        {
            return null;
        }

        void IUsersRepository.AddUser(User user)
        {
        }

        void IUsersRepository.UpdateUser(User user)
        {
        }

        void IUsersRepository.DeleteUser(User user)
        {
        }


        private User ExecuteQueryMapUser(string query, SqlParameter[] sqlParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(sqlParameters);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                UserName = reader["UserName"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
