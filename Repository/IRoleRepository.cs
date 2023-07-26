using Task3.Models;

namespace Task3.Repository
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetRoles();
        Role GetRoleById(int roleId);
        void CreateRole(Role role);
       void UpdateRole(Role role); 
        void DeleteRole(int roleId);
        IEnumerable<UserRole> GetUserRoles(int employeeId);
        //void AssignUserRole(RolePermissionsViewModel rolePermissionsViewModel);
        //void RemoveUserRole(int userRoleId);
    }
}
