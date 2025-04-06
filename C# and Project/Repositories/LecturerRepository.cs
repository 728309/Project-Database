using Microsoft.Data.SqlClient;
using C__and_Project.Models;
using System.Data;
using System.Reflection.Metadata;

namespace C__and_Project.Repositories
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly string _connectionString;

        public LecturerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            string query = "SELECT * FROM Lecturer ORDER BY LastName";

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
                                lecturers.Add(new Lecturer
                                {
                                    LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                    LecturerNumber = Convert.ToInt32(reader["lecturerNumber"]),
                                    FirstName = reader["firstName"].ToString(),
                                    LastName = reader["lastName"].ToString(),
                                    Room = Convert.ToInt32(reader["room"]),
                                    PhoneNumber = Convert.ToInt32(reader["phoneNumber"]),
                                    DateofBirth = Convert.ToDateTime(reader["dateOfBirth"]),
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving lecturers", ex);
                    }
                }
            }
            return lecturers;
        }

        public Lecturer? GetLecturerByID(int lecturerID)
        {
            string query = "SELECT * FROM Lecturer WHERE lecturerID = @lecturerID";
            SqlParameter[] sqlParameters = { new SqlParameter("@lecturerID", SqlDbType.Int) { Value = lecturerID } };

            return ExecuteQueryMapLecturer(query, sqlParameters);
        }
        public void AddLecturer(Lecturer lecturer)
        {
            string checkquery = "SELECT COUNT(*) FROM Lecturer WHERE LecturerID = @LecturerID";

            string query = "INSERT INTO Lecturer (lecturerNumber, firstName, lastName, room, phoneNumber, dateofBirth) " +
                           "VALUES (@lecturerNumber, @firstName, @lastName, @room, @phoneNumber, @dateofBirth);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand Checkcommand = new SqlCommand(checkquery, connection))
                {
                    Checkcommand.Parameters.AddWithValue("@lecturerID", lecturer.LecturerID);
                    int count = (int)Checkcommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("A lecturer with the same student number already exists.");
                    }
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lecturerNumber", lecturer.LecturerNumber);
                    command.Parameters.AddWithValue("@firstName", lecturer.FirstName);
                    command.Parameters.AddWithValue("@lastName", lecturer.LastName);
                    command.Parameters.AddWithValue("@room", lecturer.Room);
                    command.Parameters.AddWithValue("@phoneNumber", lecturer.PhoneNumber);
                    command.Parameters.AddWithValue("@dateofBirth", lecturer.DateofBirth);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected != 1)
                    {
                        throw new Exception("Adding lecturer failed!");
                    }
                }
            }
        }
        public void UpdateLecturer(Lecturer lecturer)
        {
            string query = "UPDATE Lecturer SET firstName = @firstName, lastName = @lastName, phoneNumber = @phoneNumber, room = @room, dateOfBirth = @dateOfBirth WHERE lecturerID = @lecturerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lecturerID", lecturer.LecturerID);
                    command.Parameters.AddWithValue("@lecturerNumber", lecturer.LecturerNumber);
                    command.Parameters.AddWithValue("@firstName", lecturer.FirstName);
                    command.Parameters.AddWithValue("@lastName", lecturer.LastName);
                    command.Parameters.AddWithValue("@room", lecturer.Room);
                    command.Parameters.AddWithValue("@phoneNumber", lecturer.PhoneNumber);
                    command.Parameters.AddWithValue("@dateOfBirth", lecturer.DateofBirth);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No lecturer records were updated.");
                }
            }
        }
        public void DeleteLecturer(Lecturer lecturer)
        {
            string query = "DELETE FROM Lecturer WHERE lecturerID = @lecturerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lecturerID", lecturer.LecturerID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No lecturer records were deleted.");
                }
            }
        }
        private Lecturer ExecuteQueryMapLecturer(string query, SqlParameter[] sqlParameters)
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
                            return new Lecturer
                            {
                                LecturerID = Convert.ToInt32(reader["lecturerID"]),
                                LecturerNumber = Convert.ToInt32(reader["lecturerNumber"]),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                Room = Convert.ToInt32(reader["room"]),
                                PhoneNumber = Convert.ToInt32(reader["phoneNumber"]),
                                DateofBirth = Convert.ToDateTime(reader["dateOfBirth"])
                            };
                        }
                    }
                }
            }
            return null;
        }
        public List<Lecturer> GetSupervisorsByActivityId(int activityId)
        {
            List<Lecturer> lecturers = new List<Lecturer>();

            // SQL query to get lecturers who supervise the given activity
            string query = @"
        SELECT l.LecturerID, l.lecturerNumber, l.firstName, l.lastName, l.room, l.phoneNumber, l.dateOfBirth
        FROM Lecturer l
        INNER JOIN Supervise s ON l.LecturerID = s.LecturerID
        WHERE s.ActivityID = @ActivityID";

            // Define parameters for SQL query
            SqlParameter[] sqlParameters = {
        new SqlParameter("@ActivityID", SqlDbType.Int) { Value = activityId }
    };
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(sqlParameters);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lecturers.Add(new Lecturer
                            {
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                LecturerNumber = Convert.ToInt32(reader["lecturerNumber"]),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                Room = Convert.ToInt32(reader["room"]),
                                PhoneNumber = Convert.ToInt32(reader["phoneNumber"]),
                                DateofBirth = Convert.ToDateTime(reader["dateOfBirth"])
                            });
                        }
                    }
                }
            }

            return lecturers;
        }
    }
}
