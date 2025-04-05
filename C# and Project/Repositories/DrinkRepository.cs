using Microsoft.Data.SqlClient;
using C__and_Project.Models;
using System.Data;
using System.Reflection.Metadata;

namespace C__and_Project.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string _connectionString;

        public DrinkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("prjdb25");
        }

        public List<Drinks> GetAllDrinks()
        {
            List<Drinks> drinks = new List<Drinks>();
            string query = "SELECT DrinkID, DrinkName, Stock, TypeDrink, VATPercentage FROM Drinks ORDER BY DrinkName";

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
                                drinks.Add(new Drinks
                                {
                                    DrinkID = Convert.ToInt32(reader["DrinkID"]),
                                    DrinkName = reader["DrinkName"].ToString(),
                                    Stock = Convert.ToInt32(reader["Stock"]),
                                    TypeDrink = reader["TypeDrink"].ToString(),
                                    VatPercentage = Convert.ToInt32(reader["VATPercentage"])
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving drinks", ex);
                    }
                }
            }
            return drinks;
        }

        public Drinks? GetDrinksByID(int drinkID)
        {
            // Removed extra comma after VATPercentage
            string query = "SELECT DrinkID, DrinkName, Stock, TypeDrink, VATPercentage FROM Drinks WHERE DrinkID = @DrinkID";
            SqlParameter[] sqlParameters = { new SqlParameter("@DrinkID", SqlDbType.Int) { Value = drinkID } };

            return ExecuteQueryMapDrink(query, sqlParameters);
        }

        public void AddDrink(Drinks drinks)
        {
            string checkQuery = "SELECT COUNT(*) FROM Drinks WHERE DrinkID = @DrinkID";
            string query = "INSERT INTO Drinks (DrinkName, TypeDrink, Stock, VATPercentage) " +
                           "VALUES (@DrinkName, @TypeDrink, @Stock, @VATPercentage);" +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@DrinkID", drinks.DrinkID);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("A drink with the same ID already exists.");
                    }
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrinkName", drinks.DrinkName);
                    command.Parameters.AddWithValue("@TypeDrink", drinks.TypeDrink);
                    command.Parameters.AddWithValue("@Stock", drinks.Stock);
                    command.Parameters.AddWithValue("@VATPercentage", drinks.VatPercentage);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            throw new Exception("Adding drink failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something didn't go as planned while adding the drink...", ex);
                    }
                }
            }
        }

        public void UpdateDrink(Drinks drinks)
        {
            string query = "UPDATE Drinks SET DrinkName = @DrinkName, TypeDrink = @TypeDrink, Stock = @Stock, VATPercentage = @VATPercentage WHERE DrinkID = @DrinkID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrinkID", drinks.DrinkID);
                    command.Parameters.AddWithValue("@DrinkName", drinks.DrinkName);
                    command.Parameters.AddWithValue("@TypeDrink", drinks.TypeDrink);
                    command.Parameters.AddWithValue("@Stock", drinks.Stock);
                    command.Parameters.AddWithValue("@VATPercentage", drinks.VatPercentage);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No drink records were updated.");
                }
            }
        }

        public void DeleteDrink(Drinks drinks)
        {
            string query = "DELETE FROM Drinks WHERE DrinkID = @DrinkID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Corrected parameter name reference
                    command.Parameters.AddWithValue("@DrinkID", drinks.DrinkID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No drink records were deleted.");
                }
            }
        }

        private Drinks ExecuteQueryMapDrink(string query, SqlParameter[] sqlParameters)
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
                            return new Drinks
                            {
                                DrinkID = Convert.ToInt32(reader["DrinkID"]),
                                DrinkName = reader["DrinkName"].ToString(),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                TypeDrink = reader["TypeDrink"].ToString(),
                                VatPercentage = Convert.ToInt32(reader["VATPercentage"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
