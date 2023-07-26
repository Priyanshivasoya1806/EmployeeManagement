using Dapper.Contrib.Extensions;

namespace Task3.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentID { get; set; }
        public decimal Salary { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
