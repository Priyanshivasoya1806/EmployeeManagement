using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Task3.Models;
using Task3.Repository;
using Microsoft.Extensions.Configuration;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly IDbConnection _dbConnection;
    

    public RolePermissionRepository(IDbConnection dbConnection, IConfiguration configuration)
    {
        _dbConnection = dbConnection;
        
        string connectionString = configuration.GetConnectionString("EmployeeManagement");
        _dbConnection.ConnectionString = connectionString;
    }


    public IEnumerable<Role> GetRoles()
    {
        using (IDbConnection connection = _dbConnection)
        {
            return connection.Query<Role>("GetRoles", commandType: CommandType.StoredProcedure);
        }
    }

    


    public void AssignRolesToUser(int EmployeeID, IEnumerable<int> roleIds)
    {
        using (IDbConnection connection = _dbConnection)
        {
            foreach (int roleId in roleIds)
            {
                var parameters = new { EmployeeID = EmployeeID, RoleID = roleId };
                connection.Execute("AssignUserRole", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }


    //public void RemoveUserRole(int userRoleId)
    //{
    //    using (IDbConnection connection = _dbConnection)
    //    {
    //        connection.Open();
    //        connection.Execute("RemoveUserRole", new { UserRoleID = userRoleId }, commandType: CommandType.StoredProcedure);
    //    }
    //}


}
