using Task3.Models;

namespace Task3.Repository
{
    public interface IPermissionRepository
    {
        IEnumerable<Permissions> GetPermissions();
        void AssignPermissionsToRole(int roleId, IEnumerable<int> permissionIds);

    }
}
