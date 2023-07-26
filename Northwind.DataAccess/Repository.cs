using Northwind.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Northwind.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected string _connectionString;
        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Delete(int id)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM " + typeof(T).Name + " WHERE Id = @Id;";
                if(connection.Execute(deleteQuery, new { Id = id })>=1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                string query = "INSERT INTO " + typeof(T).Name + " VALUES /**/;";
                return connection.Execute(query, entity);
            }
        }

        public bool Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE " + typeof(T).Name + " SET /**/ WHERE Id = @Id;";
                if (connection.Execute(query, entity) >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
