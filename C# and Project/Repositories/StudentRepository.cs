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
            string query = "SELECT * FROM Student ORDER BY lastName";

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
                                    StudentID = Convert.ToInt32(reader["studentID"]),
                                    StudentNumber = Convert.ToInt32(reader["studentNumber"]),
                                    Room = Convert.ToInt32(reader["room"]),
                                    Date = Convert.ToDateTime(reader["date"]),
                                    FirstName = reader["firstName"].ToString(),
                                    LastName = reader["lastName"].ToString(),
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
            string query = "SELECT studentID, studentNumber, room, date, firstName, lastName FROM Student WHERE studentID = @studentID";
            SqlParameter[] sqlParameters = { new SqlParameter("@studentID", SqlDbType.Int) { Value = studentID } };

            return ExecuteQueryMapStudent(query, sqlParameters);
        }

        public void AddStudent(Student student)
        {
            string checkQuery = "SELECT COUNT(*) FROM Student WHERE studentID = @studentID";
            string insertQuery = "INSERT INTO Student (firstName, lastName, date, room, studentNumber) " +
                                 "VALUES (@firstName, @lastName, @date, @room, @studentNumber);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@studentID", student.StudentID);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("A student with the same student ID already exists.");
                    }
                }

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@firstName", student.FirstName);
                    command.Parameters.AddWithValue("@lastName", student.LastName); 
                    command.Parameters.AddWithValue("@date", student.Date);
                    command.Parameters.AddWithValue("@room", student.Room);
                    command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected != 1)
                    {
                        throw new Exception("Adding student failed!");
                    }
                }
            }
        }

        public void UpdateStudent(Student student)
        {
          string query = "UPDATE Student SET firstName = @firstName, lastName = @lastName, " +
                "room = @roomID, date = @date, studentNumber = @studentNumber WHERE studentID = @studentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentID", student.StudentID);
                    command.Parameters.AddWithValue("@firstName", student.FirstName);
                    command.Parameters.AddWithValue("@lastName", student.LastName);
                    command.Parameters.AddWithValue("@roomID", student.Room);
                    command.Parameters.AddWithValue("@date", student.Date);
                    command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No student records updated!");
                }
            }
        }

        public void DeleteStudent(Student student )
        {
            string query = "DELETE FROM Student WHERE studentID = @studentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentID", student.StudentID);

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
                                StudentID = Convert.ToInt32(reader["studentID"]),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                Room = Convert.ToInt32(reader["room"]),
                               Date = Convert.ToDateTime(reader["date"]),
                               StudentNumber = Convert.ToInt32(reader["studentNumber"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
