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
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT * FROM Student ORDER BY LastName";

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
                                    StudentNmbr = Convert.ToInt32(reader["StudentNmbr"]),
                                    Room = Convert.ToInt32(reader["Room"]),
                                    Date = Convert.ToDateTime(reader["DateTime"]),
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
            string query = "SELECT StudentID, StudentNmbr, Room, DateTime, FirstName, LastName FROM Student WHERE StudentID = @StudentID";
            SqlParameter[] sqlParameters = { new SqlParameter("@StudentID", SqlDbType.Int) { Value = studentID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddStudent(Student student)
        {
            string checkquery = "SELECT COUNT(*) FROM Student WHERE StudentID = @StudentID";

            string query = "INSERT INTO Student (FirstName, LastName, DateTime, Room, StudentNmbr ) " +
                           "VALUES (@FirstName, @LastName, @DateTIme, @Room, @StudentNmbr );" +
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
                    command.Parameters.AddWithValue("@RoomID", student.Room);
                    command.Parameters.AddWithValue("@StudentNmbr", student.StudentNmbr);

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
                           "RoomID = @RoomID, DateTime = @DateTime WHERE StudentID = @StudentID, StudentNmbr = @StudentNmbr";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@RoomID", student.Room);
                    command.Parameters.AddWithValue("@DateTime", student.Date);
                    command.Parameters.AddWithValue("@StudentNmbr", student.StudentNmbr);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No student records updated!");
                }
            }
        }

        public void DeleteStudent(Student student )
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
                                Room = Convert.ToInt32(reader["Room"]),
                               Date = Convert.ToDateTime(reader["DateTime"]),
                               StudentNmbr = Convert.ToInt32(reader["StudentNmbr"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
