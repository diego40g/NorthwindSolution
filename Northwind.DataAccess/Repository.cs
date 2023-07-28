using Northwind.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System;

namespace Northwind.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected string _connectionString;
        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Delete(T entity)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var primaryKeyProperty = typeof(T).GetProperty("Id");
                if(primaryKeyProperty == null)
                {
                    throw new InvalidOperationException("The entity does not have a primary key 'Id'.");
                }

                int idValue = (int )primaryKeyProperty.GetValue(entity);
                string deleteQuery = $"DELETE FROM {typeof(T).Name} WHERE Id=@Id";

                int rowsAffected = connection.Execute(deleteQuery, new {Id=idValue});

                return rowsAffected > 0;
            }
        }

        public T GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM " + typeof(T).Name + " WHERE Id = @Id;";

                return connection.QuerySingleOrDefault<T>(query, new { Id = id });
            }
        }

        public IEnumerable<T> GetList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM " + typeof(T).Name + ";";
                return connection.Query<T>(query);
            }
        }

        public int Insert(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                var properties = typeof(T).GetProperties();
                var insertQuery = $"INSERT INTO {typeof(T).Name} (";
                var insertValues = "VALUES (";
                var parameters = new DynamicParameters();

                foreach(var property in properties)
                {
                    if (property.Name != "Id")
                    {
                        insertQuery += $"{property.Name}, ";
                        insertValues += $"@{property.Name}, ";
                        parameters.Add($"@{property.Name}", property.GetValue(entity));
                    }
                }

                insertQuery = insertQuery.TrimEnd(',', ' ');
                insertQuery += ") ";

                insertValues = insertValues.TrimEnd(',', ' ');
                insertValues += ")";

                insertQuery += insertValues;

                int rowsAffected = connection.Execute(insertQuery, parameters);

                return rowsAffected;
            }
        }

        public bool Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var properties = typeof(T).GetProperties();
                var updateQuery = $"UPDATE {typeof(T).Name} SET ";
                var parameters = new DynamicParameters();

                foreach(var property in properties)
                {
                    if (property.Name != "Id")
                    {
                        updateQuery += $"{property.Name} = @{property.Name}, ";
                        parameters.Add($"@{property.Name}", property.GetValue(entity));
                    }
                }

                updateQuery = updateQuery.TrimEnd(',', ' ');
                updateQuery += " WHERE Id = @Id";
                parameters.Add("@Id", typeof(T).GetProperty("Id").GetValue(entity));


                int rowsAffected = connection.Execute(updateQuery, parameters);

                return rowsAffected > 0;
            }
        }
    }
}
