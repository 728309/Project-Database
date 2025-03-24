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
            _connectionString = configuration.GetConnectionString("C#andProjectDatabase");
        }

        public List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            string query = "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateofBirth FROM Lecturers ORDER BY LastName";

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
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                                    DateofBirth = reader["DateofBirth"].ToString()
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

        public Lecturer? GetLecturerByID(int LecturerID)
        {
            string query = "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateofBirth FROM Lecturers WHERE LecturerID = @LecturerID";
            SqlParameter[] sqlParameters = { new SqlParameter("@LecturerID", SqlDbType.Int) { Value = LecturerID } };

            return ExecuteQueryMapLecturer(query, sqlParameters);
        }
        public void AddLecturer(Lecturer lecturer)
        {
            string checkquery = "SELECT COUNT(*) FROM Lecturers WHERE LecturerID = @LecturerID";

            string query = "INSERT INTO Lecturers (FirstName, LastName, PhoneNumber, DateofBirth) " +
                           "VALUES (@FirstName, @LastName, @PhoneNumber, @DateofBirth);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand Checkcommand = new SqlCommand(checkquery, connection))
                {
                    Checkcommand.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                    int count = (int)Checkcommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("A lecturer with the same student number already exists.");
                    }
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                    command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                    command.Parameters.AddWithValue("@DateofBirth", lecturer.DateofBirth);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            throw new Exception("Adding lecturer failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something didn't go as planned...", ex);
                    }
                }
            }
        }
        public void UpdateLecturer(Lecturer lecturer)
        {
            string query = "UPDATE Lecturers SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, DateofBirth = @DateofBirth WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                    command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                    command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                    command.Parameters.AddWithValue("@DateofBirth", lecturer.DateofBirth);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No lecturer records were updated.");
                }
            }
        }
        public void DeleteLecturer(Lecturer lecturer)
        {
            string query = "DELETE FROM Lecturers WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);

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
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                                DateofBirth = reader["DateofBirth"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
