namespace Task3.Models
{
    public class RolePermissionsViewModel
    {
        public int EmployeeID { get; set; }
        public List<Role> AvailableRoles { get; set; }
        public int SelectedRole { get; set; }
        public UserRole UserRole { get; set; }
    }
}
