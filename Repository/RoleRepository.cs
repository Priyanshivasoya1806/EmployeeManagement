using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Task3.Models;

namespace Task3.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbConnection _dbConnection;


        public RoleRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;

            string connectionString = configuration.GetConnectionString("EmployeeManagement");
            _dbConnection.ConnectionString = connectionString;
        }

        public IEnumerable<Role> GetRoles()
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                var roles = connection.Query<Role>("GetRoles", commandType: CommandType.StoredProcedure);
                return roles;
            }
        }

        public Role GetRoleById(int roleId)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                var role = connection.QuerySingleOrDefault<Role>("GetRoleById", new { RoleId = roleId }, commandType: CommandType.StoredProcedure);
                return role;
            }
        }

        public void CreateRole(Role role)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                connection.Execute("CreateRole", new { RoleName = role.RoleName }, commandType: CommandType.StoredProcedure);
            }
        }

    
        public void UpdateRole(Role role)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                connection.Execute("UpdateRole", role, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteRole(int roleId)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                connection.Execute("DeleteRole", new { RoleId = roleId }, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<UserRole> GetUserRoles(int employeeId)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Open();
                var userRoles = connection.Query<UserRole>("GetUserRoles", new { EmployeeID = employeeId }, commandType: CommandType.StoredProcedure);
                return userRoles;
            }
        }
    }
}
