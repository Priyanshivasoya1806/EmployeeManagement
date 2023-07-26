using Task3.Models;

namespace Task3.Repository;
    public interface IEmployeeRepository
{

    Task<bool> UpdateAsync(int employeeId, string firstName, string lastName, int departmentId, decimal salary, string Email,string Password, string UserName);
    Task<bool> DeleteAsync(int employeeId);
    Task<Employee?> GetByIdAsync(int id);
    Task<int> GetTotalRecords(string searchValue); 
    Task<IEnumerable<Employee>> GetAllEmployee(int startIndex, int pageSize, string searchValue, string sortColumn, string sortDirection);
    Task<bool> AddAsync(string firstName, string lastName, int departmentId, decimal salary, string Email,string Password, string UserName);
    
}

