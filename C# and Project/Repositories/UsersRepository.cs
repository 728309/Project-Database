using Microsoft.Data.SqlClients;
using C__and_Project.Models;
using System.Data;

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
            string query = "SELECT UserID, UserName, MobileNumber, EmailAddress FROM Users WHERE UserID = @UserID";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = userId }
            };

            return ExecuteQueryMapUser(query, sqlParameters);
        }

        void IUsersRepository.AddUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (UserName, MobileNumber, EmailAddress) " +
                               "VALUES (@UserName, @MobileNumber, @EmailAddress);" + "SELECT SCOPE_IDENTITY()";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                    //hash password in separate service before storing! 
                    // command.Parameters.AddWithValue("@Password", user.Password); 

                    try
                    {
                        connection.Open();

                        //get last inserted record identity value back 
                        //user.UserID = Convert.ToInt32(command.ExecuteScalar());

                        int nrOfRowsAffected = command.ExecuteNonQuery();

                        if (nrOfRowsAffected != 1)
                        {
                            throw new Exception("Adding user failed!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something went wrong", ex);
                    }
                }
            }
        }

        void IUsersRepository.UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET UserName = @Name, MobileNumber = @MobileNumber, " +
                               "EmailAddress = @EmailAddress WHERE UserId = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.UserID);
                command.Parameters.AddWithValue("@Name", user.UserName);
                command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        void IUsersRepository.DeleteUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Users WHERE UserId = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.UserID);

                    connection.Open();
                    int nrOfRowsAffected = command.ExecuteNonQuery();

                    if (nrOfRowsAffected == 0)
                    {
                        throw new Exception("No records deleted!");
                    }
                }
            }
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