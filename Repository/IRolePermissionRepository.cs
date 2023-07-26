using Task3.Models;

namespace Task3.Repository
{
    public interface IRolePermissionRepository
    {
        IEnumerable<Role> GetRoles();
       
        void AssignRolesToUser(int EmployeeID, IEnumerable<int> roleIds);
    }
}
