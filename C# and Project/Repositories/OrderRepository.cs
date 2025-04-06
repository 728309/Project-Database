using Microsoft.Data.SqlClient;
using C__and_Project.Models;
using System.Data;

namespace C__and_Project.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM DrinkOrder ORDER BY DrinkId";

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
                                orders.Add(new Order
                                {
                                    StudentId = Convert.ToInt32(reader["StudentId"]),
                                    DrinkId = Convert.ToInt32(reader["DrinkId"]),
                                    Amount = Convert.ToInt32(reader["Amount"]),
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving orders", ex);
                    }
                }
            }
            return orders;
        }

        public Order? GetOrderByID(int studentId, int drinkId)
        {
            string query = "SELECT StudentId, DrinkId, Amount FROM DrinkOrder WHERE StudentId = @StudentId AND DrinkId = @DrinkId";
            SqlParameter[] sqlParameters = {
                new SqlParameter("@StudentId", SqlDbType.Int) { Value = studentId },
                new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
            };

            return ExecuteQueryMapOrder(query, sqlParameters);
        }

        public void AddOrder(Order order)
        {
            string query = "INSERT INTO DrinkOrder (StudentId, DrinkId, Amount) " +
                           "VALUES (@StudentId, @DrinkId, @Amount);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", order.StudentId);
                    command.Parameters.AddWithValue("@DrinkId", order.DrinkId);
                    command.Parameters.AddWithValue("@Amount", order.Amount);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            throw new Exception("Adding order failed!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something went wrong while adding order", ex);
                    }
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            string query = "UPDATE DrinkOrder SET Amount = @Amount WHERE StudentId = @StudentId AND DrinkId = @DrinkId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", order.StudentId);
                    command.Parameters.AddWithValue("@DrinkId", order.DrinkId);
                    command.Parameters.AddWithValue("@Amount", order.Amount);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No order records updated!");
                }
            }
        }

        public void DeleteOrder(Order order)
        {
            string query = "DELETE FROM DrinkOrder WHERE StudentId = @StudentId AND DrinkId = @DrinkId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", order.StudentId);
                    command.Parameters.AddWithValue("@DrinkId", order.DrinkId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No order records deleted!");
                }
            }
        }

        private Order ExecuteQueryMapOrder(string query, SqlParameter[] sqlParameters)
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
                            return new Order
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                DrinkId = Convert.ToInt32(reader["DrinkId"]),
                                Amount = Convert.ToInt32(reader["Amount"]),
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
