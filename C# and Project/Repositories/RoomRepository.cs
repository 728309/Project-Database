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

        List<Room> IRoomRepository. GetAllRooms()
        {
            List<Room> room = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Room ORDER BY roomNumber";
                SqlCommand command = new SqlCommand(query, connection);
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                room.Add(new Room
                                {
                                    RoomID = Convert.ToInt32(reader["roomID"]),
                                    TypeRoom = reader["typeRoom"].ToString(),
                                    Capacity = Convert.ToInt32(reader["capacity"]),
                                    RoomNumber = reader["roomNumber"].ToString()
                                   
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
            string query = "SELECT roomID, typeRoom, capacity, roomNumber FROM Room WHERE roomID = @roomID";
            SqlParameter[] sqlParameters = { new SqlParameter("@roomID", SqlDbType.Int) { Value = RoomID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddRoom(Room room)
        {
            string checkquery = "SELECT COUNT(*) FROM Room WHERE roomID = @roomID";

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
                    command.Parameters.AddWithValue("@typeRoom", room.TypeRoom);
                    command.Parameters.AddWithValue("@capacity", room.Capacity);
                    command.Parameters.AddWithValue("@roomNumber", room.RoomNumber);

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
            string query = "UPDATE Room SET typeRoom = @TypeRoom, capacity = @Capacity, " +
               "roomNumber = @RoomNumber WHERE roomID = @RoomID"; 

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
            string query = "DELETE FROM Room WHERE roomID = @RoomID";

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