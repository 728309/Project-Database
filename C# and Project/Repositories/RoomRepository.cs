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
                                Room.Add(new room
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    TypeRoom = reader["RoomType"].ToString(),
                                    Capacity = Convert.ToInt32(reader["RoomID"]),
                                    RoomNumber = reader["RoomNumber"].ToString()
                                   
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving rooms", ex);
                    }
                }
            }
            return room;
        }
        
        public Room? GetRoomById(int RoomID)
        {
            string query = "SELECT RoomID, TypeRoom, Capacity, RoomNumber WHERE roomID = @roomID";
            SqlParameter[] sqlParameters = { new SqlParameter("@roomID", SqlDbType.Int) { Value = RoomID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddRoom(Room room)
        {
            string checkquery = "SELECT COUNT(*) FROM Student WHERE roomID = @roomID";

            string query = "INSERT INTO Room( roomID, typeRoom, capacity, roomNumber) " +
                           "VALUES (@roomID, @typeRoom, @capacity, @roomNumber);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand Checkcommand = new SqlCommand(checkquery, connection))
                {
                    Checkcommand.Parameters.AddWithValue("@roomID", room.RoomID);
                    int count = (int)Checkcommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("The room is unavailble.");
                    }
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@roomID", room.RoomID);
                    command.Parameters.AddWithValue("@LastName", room.TypeRoom);
                    command.Parameters.AddWithValue("@DateTime", room.Capacity);
                    command.Parameters.AddWithValue("@RoomID", room.RoomNumber);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            throw new Exception("Adding room failed!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something went wrong", ex);
                    }
                }
            }
        }

        public void UpdateRoom(Room room)
        {
            string query = "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, " +
                           "RoomID = @RoomID, DateTime = @DateTime WHERE StudentID = @StudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", room.RoomID);
                    command.Parameters.AddWithValue("@TypeRoom", room.TypeRoom);
                    command.Parameters.AddWithValue("@Capacity", room.Capacity);
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No Room records updated!");
                }
            }
        }

        public void DeleteRoom(Room room)
        {
            string query = "DELETE FROM Student WHERE StudentID = @StudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", room.RoomID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No room records deleted!");
                }
            }
        }

        private Room ExecuteQueryMapStudent(string query, SqlParameter[] sqlParameters)
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
                            return new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                TypeRoom = reader["RoomType"].ToString(),
                                Capacity = Convert.ToInt32(reader["RoomID"]),
                                RoomNumber = reader["RoomNumber"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}