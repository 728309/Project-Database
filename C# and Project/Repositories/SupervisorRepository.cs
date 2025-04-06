using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using C__and_Project.Models;

namespace C__and_Project.Repositories
{
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly string _connectionString;

        public SupervisorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public void AddSupervisorToActivity(int activityId, int lecturerId)
        {
            string query = "INSERT INTO Supervise (ActivityID, LecturerID) VALUES (@ActivityID, @LecturerID)";

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityID", activityId);
            command.Parameters.AddWithValue("@LecturerID", lecturerId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void RemoveSupervisorFromActivity(int activityId, int lecturerId)
        {
            string query = "DELETE FROM Supervise WHERE ActivityID = @ActivityID AND LecturerID = @LecturerID";

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityID", activityId);
            command.Parameters.AddWithValue("@LecturerID", lecturerId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<Lecturer> GetAvailableLecturersForActivity(int activityId)
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            string query = @"
                SELECT * FROM Lecturer 
                WHERE LecturerID NOT IN (
                    SELECT LecturerID FROM Supervise WHERE ActivityID = @ActivityID
                )";

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityID", activityId);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lecturers.Add(new Lecturer
                {
                    LecturerID = Convert.ToInt32(reader["LecturerID"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Room = Convert.ToInt32(reader["Room"]),
                    PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                    DateofBirth = Convert.ToDateTime(reader["DateofBirth"]),
                });
            }

            return lecturers;
        }
    }
}
