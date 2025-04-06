using C__and_Project.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace C__and_Project.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25"); // Ensure your connection string is correctly configured
        }

        public IEnumerable<Activity> GetAllActivities()
        {
            List<Activity> activities = new List<Activity>();
            string query = "SELECT * FROM Activity ORDER BY Date, StartTime";

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
                                activities.Add(new Activity
                                {
                                    ActivityID = Convert.ToInt32(reader["ActivityID"]),
                                    Name = reader["Name"].ToString(),
                                    Date = Convert.ToDateTime(reader["Date"]),
                                    StartTime = (TimeSpan)reader["StartTime"],
                                    Duration = Convert.ToInt32(reader["Duration"])
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving activities", ex);
                    }
                }
            }
            return activities;
        }

        // Get a specific activity by ID
        public Activity? GetActivityById(int activityId)
        {
            string query = "SELECT * FROM Activity WHERE ActivityID = @ActivityID";
            SqlParameter[] sqlParameters = { new SqlParameter("@ActivityID", SqlDbType.Int) { Value = activityId } };

            return ExecuteQueryMapActivity(query, sqlParameters);
        }

        // Add a new activity to the database
        public void AddActivity(Activity activity)
        {
            string query = "INSERT INTO Activity (Name, Date, StartTime, Duration) " +
                           "VALUES (@Name, @Date, @StartTime, @Duration);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@Duration", activity.Duration);

                    try
                    {
                        connection.Open();
                        int activityId = Convert.ToInt32(command.ExecuteScalar());
                        activity.ActivityID = activityId;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while adding activity", ex);
                    }
                }
            }
        }

        // Update an existing activity in the database
        public void UpdateActivity(Activity activity)
        {
            string query = "UPDATE Activity SET Name = @Name, Date = @Date, StartTime = @StartTime, Duration = @Duration WHERE ActivityID = @ActivityID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                    command.Parameters.AddWithValue("@Duration", activity.Duration);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Activity not found for updating");
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while updating activity", ex);
                    }
                }
            }
        }

        // Delete an activity from the database
        public void DeleteActivity(int activityId)
        {
            string query = "DELETE FROM Activity WHERE ActivityID = @ActivityID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", activityId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Activity not found for deletion");
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while deleting activity", ex);
                    }
                }
            }
        }

        // Helper method to execute query and map results to Activity object
        private Activity ExecuteQueryMapActivity(string query, SqlParameter[] sqlParameters)
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
                            return new Activity
                            {
                                ActivityID = Convert.ToInt32(reader["ActivityID"]),
                                Name = reader["Name"].ToString(),
                                Date = Convert.ToDateTime(reader["Date"]),
                                StartTime = (TimeSpan)reader["StartTime"],
                                Duration = Convert.ToInt32(reader["Duration"]),
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Optionally, implement a method for getting supervisors for a specific activity
        public List<Lecturer> GetSupervisorsByActivityId(int activityId)
        {
            List<Lecturer> supervisors = new List<Lecturer>();
            string query = "SELECT l.LecturerID, l.FirstName, l.LastName, l.Room, l.PhoneNumber, l.DateofBirth " +
                           "FROM Lecturer l " +
                           "JOIN Supervise s ON l.LecturerID = s.LecturerID " +
                           "WHERE s.ActivityID = @ActivityID";

            SqlParameter[] sqlParameters = { new SqlParameter("@ActivityID", SqlDbType.Int) { Value = activityId } };

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
                            supervisors.Add(new Lecturer
                            {
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Room = Convert.ToInt32(reader["Room"]),
                                PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                                DateofBirth = Convert.ToDateTime(reader["DateofBirth"]),
                            });
                        }
                    }
                }
            }
            return supervisors;
        }
        public bool ActivityExists(string name)
        {
            string query = "SELECT COUNT(*) FROM Activity WHERE Name = @Name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;  // If count is greater than 0, the activity already exists
                }
            }
        }

        public List<Student> GetParticipantsByActivityId(int activityId)
        {
            List<Student> students = new List<Student>();
            string query = @"
                SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.Room, s.Date
                FROM Student s
                JOIN Particpate ON s.StudentID = Particpate.StudentID
                WHERE Particpate.ActivityID = @ActivityID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            StudentNumber = Convert.ToInt32(reader["StudentNumber"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Room = Convert.ToInt32(reader["Room"]),
                            Date = Convert.ToDateTime(reader["Date"])
                        });
                    }
                }
            }

            return students;
        }


        public List<Student> GetAvailableStudents(int activityId)
        {
            List<Student> students = new List<Student>();
            string query = @"
                    SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.Room, s.Date
                    FROM Student s
                    WHERE s.StudentID NOT IN (
                        SELECT StudentID FROM Particpate WHERE ActivityID = @ActivityID
                    )";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            StudentNumber = Convert.ToInt32(reader["StudentNumber"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Room = Convert.ToInt32(reader["Room"]),
                            Date = Convert.ToDateTime(reader["Date"])
                        });
                    }
                }
            }

            return students;
        }

        public void AddStudentToActivity(int activityId, int studentId)
        {
            string query = "INSERT INTO Particpate (ActivityID, StudentID) VALUES (@ActivityID, @StudentID)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveStudentFromActivity(int activityId, int studentId)
        {
            string query = "DELETE FROM Particpate WHERE ActivityID = @ActivityID AND StudentID = @StudentID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }



    }
}
