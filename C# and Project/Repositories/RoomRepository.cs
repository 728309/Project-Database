using System.Data;
using C__and_Project.Models;
using Microsoft.Data.SqlClient;

namespace C__and_Project.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly string _connectionString;

        public RoomRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public List<Room> GetAllRooms()
        {
            List<Room> room = new List<Room>();
            string query = "SELECT * FROM Room ORDER BY RoomNumber";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Room.Add(new Room
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    RoomType = reader["RoomType"].ToString(),
                                    Capacity = Convert.ToInt32(reader["RoomID"]),
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                   
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving students", ex);
                    }
                }
            }
            return Room;
        }

        public Room? GetRootByID(int RoomID)
        {
            string query = "SELECT RoomID, RoomType, Capacity, RoomNumber WHERE roomID = @roomID";
            SqlParameter[] sqlParameters = { new SqlParameter("@roomID", SqlDbType.Int) { Value = RoomID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddRoom(Room room)
        {
            string checkquery = "SELECT COUNT(*) FROM Student WHERE roomID = @roomID";

            string query = "INSERT INTO Room(, LastName, DateTime, RoomID) " +
                           "VALUES (@FirstName, @LastName, @DateTIme, @RoomID);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand Checkcommand = new SqlCommand(checkquery, connection))
                {
                    Checkcommand.Parameters.AddWithValue("@StudentID", student.StudentID);
                    int count = (int)Checkcommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("A student with the same student number already exists.");
                    }
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateTime", student.Date);
                    command.Parameters.AddWithValue("@RoomID", student.RoomID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            throw new Exception("Adding student failed!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something went wrong", ex);
                    }
                }
            }
        }

        public void UpdateStudent(Student student)
        {
            string query = "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, " +
                           "RoomID = @RoomID, DateTime = @DateTime WHERE StudentID = @StudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@RoomID", student.RoomID);
                    command.Parameters.AddWithValue("@DateTime", student.Date);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No student records updated!");
                }
            }
        }

        public void DeleteStudent(Student student)
        {
            string query = "DELETE FROM Student WHERE StudentID = @StudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No student records deleted!");
                }
            }
        }

        private Student ExecuteQueryMapStudent(string query, SqlParameter[] sqlParameters)
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
                            return new Student
                            {
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                Date = Convert.ToDateTime(reader["DateTime"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}