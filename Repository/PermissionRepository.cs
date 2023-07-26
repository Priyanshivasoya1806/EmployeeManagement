using Dapper;
using System.Data;
using Task3.Models;

namespace Task3.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbConnection _dbConnection;


        public PermissionRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;

            string connectionString = configuration.GetConnectionString("EmployeeManagement");
            _dbConnection.ConnectionString = connectionString;
        }

        public IEnumerable<Permissions> GetPermissions()
        {
            using (IDbConnection connection = _dbConnection)
            {
                return connection.Query<Permissions>("GetPermissions", commandType: CommandType.StoredProcedure);
            }
        }

        public void AssignPermissionsToRole(int roleId, IEnumerable<int> permissionIds)
        {
            using (IDbConnection connection = _dbConnection)
            {
                connection.Execute("AssignPermissionsToRole", permissionIds.Select(permissionId => new { RoleID = roleId, PermissionID = permissionId }), commandType: CommandType.StoredProcedure);
            }
        }
    }
}
