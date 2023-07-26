using Dapper;
using Northwind.Models;
using Northwind.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Northwind.DataAccess
{
    public class CustomerRepository: Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(string connectionString): base(connectionString) 
        {
            
        }

        public IEnumerable<Customer> CustomerPagedList(int page, int rows)
        {
            var paremeters = new DynamicParameters();
            paremeters.Add("@page", page);
            paremeters.Add("@rows", rows);

            using(var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Customer>("dbo.CustomerPagedList",
                                                    paremeters,
                                                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
