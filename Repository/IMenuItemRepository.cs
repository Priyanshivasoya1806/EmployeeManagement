using Task3.Models;

namespace Task3.Repository
{
    public interface IMenuItemRepository
    {
            List<MenuItem> GetMenuItemsByRole(string roleName);
    }
}
