using Microsoft.Data.SqlClient;
using C__and_Project.Models;
using System.Data;

namespace C__and_Project.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("C#andProjectDatabase");
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT StudentID, RoomID , DateTime, FirstName, LastName FROM Students ORDER BY LastName";

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
                                students.Add(new Student
                                {
                                    StudentID = Convert.ToInt32(reader["StudentID"]),
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                   // DateTime = DateTime Now,
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
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
            return students;
        }

        public Student? GetStudentByID(int studentID)
        {
            string query = "SELECT StudentID, RoomID, DateTime, FirstName, LastName FROM Students WHERE StudentID = @StudentID";
            SqlParameter[] sqlParameters = { new SqlParameter("@StudentID", SqlDbType.Int) { Value = studentID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddStudent(Student student)
        {
            string query = "INSERT INTO Students (FirstName, LastName, DateTime, RoomID) " +
                           "VALUES (@FirstName, @LastName, @DateTIme, @RoomID);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateTime", student.DateTime);
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
            string query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, " +
                           "RoomID = @RoomID, DateTime = @DateTime WHERE StudentID = @StudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@RoomID", student.RoomID);
                    command.Parameters.AddWithValue("@DateTime", student.DateTime);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No student records updated!");
                }
            }
        }

        public void DeleteStudent(Student student )
        {
            string query = "DELETE FROM Students WHERE StudentID = @StudentID";

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
                               // DateTime = Convert.(reader["DateTime"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
