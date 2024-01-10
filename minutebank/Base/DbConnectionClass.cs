using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace minutebank.Base
{
    public class DbConnectionClass : IDbConnectionClass
    {
        private readonly string? _connectionString;

        public DbConnectionClass(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<T> GetEntities<T>(string query, Func<SqlDataReader, T> mapFunc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var entities = new List<T>();

                        while (reader.Read())
                        {
                            T entity = mapFunc(reader);
                            entities.Add(entity);
                        }

                        return entities;
                    }
                }
            }
        }


        public int AddEntity<T>(string query,Dictionary<string, object> dictionary)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var parameter in dictionary)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    command.CommandText += "SELECT CAST(scope_identity() AS int)";
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int generatedKey))
                    {
                        return generatedKey;
                    }

                    return 0; // Return 0 if there is an issue
                }
            }
        }

        public T? GetEntity<T>(string query, Func<SqlDataReader, T> mapFunc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            T entity = mapFunc(reader);
                            return entity;
                        }

                        return default;
                    }
                }
            }
        }

        public void UpdateEntity<T>(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<T> ExecuteStoredProcedure<T>(string storedProcedureName, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapFunc)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var entities = new List<T>();

                        while (reader.Read())
                        {
                            T entity = mapFunc(reader);
                            entities.Add(entity);
                        }

                        return entities;
                    }
                }
            }
        }

    }
}
