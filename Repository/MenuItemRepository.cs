using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Task3.Models;

namespace Task3.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly IDbConnection _dbConnection;


        public MenuItemRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;

            string connectionString = configuration.GetConnectionString("EmployeeManagement");
            _dbConnection.ConnectionString = connectionString;
        }

        public List<MenuItem> GetMenuItemsByRole(string roleName)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();

                var parameters = new { RoleAccess = roleName };

                return connection.Query<MenuItem>("GetMenuItemsByRole", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }


    }
}
