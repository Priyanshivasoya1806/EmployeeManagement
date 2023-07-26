using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.Common;
using Task3.Models;

namespace Task3.Repository;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnection _dbConnection;
 

    public EmployeeRepository(IDbConnection dbConnection, IConfiguration configuration)
    {
        _dbConnection = dbConnection;

        string connectionString = configuration.GetConnectionString("EmployeeManagement");
        _dbConnection.ConnectionString = connectionString;
    }
    //string query = "SELECT * FROM Employee";
    //return await _dbConnection.QueryAsync<Employee>(query);
    public async Task<IEnumerable<Employee>> GetAllEmployee(int startIndex, int pageSize, string searchValue, string sortColumn, string sortDirection)
    {
       
        using (IDbConnection connection = _dbConnection)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startIndex", startIndex);
            parameters.Add("@pageSize", pageSize);
            parameters.Add("@searchValue", searchValue);
            parameters.Add("@sortColumn", sortColumn);
            parameters.Add("@sortDirection", sortDirection);
            parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await connection.QueryAsync<Employee>("sspProcedure", parameters, commandType: CommandType.StoredProcedure);

            var employees = result.AsList();
            var totalRecords = parameters.Get<int>("@totalRecords");

            var employeeListingModel = new DataTableParameters
            {
                StartIndex = startIndex,
                PageSize = pageSize,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                Employees = employees,
                TotalRecords = totalRecords,
                CurrentPage = (startIndex / pageSize) + 1,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            };

            return employeeListingModel.Employees;
        }
    }

    public async Task<int> GetTotalRecords(string searchValue)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@searchValue", searchValue);
            parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("GetTotalRecordsProcedure", parameters, commandType: CommandType.StoredProcedure);

            var totalRecords = parameters.Get<int>("@totalRecords");
            return totalRecords;
        }
        catch (Exception ex)
        {
           
            return 0; 
        }
    }



    public async Task<bool> AddAsync(string firstName, string lastName, int departmentId, decimal salary, string Email,string Password, string UserName)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@DepartmentID", departmentId);
            parameters.Add("@Salary", salary);
            parameters.Add("@Email", Email);
            parameters.Add("Password", Password);
            parameters.Add("@UserName", UserName);

            await _dbConnection.ExecuteAsync("AddEmployee", parameters, commandType: CommandType.StoredProcedure);

            return true;
        }
        catch (Exception ex)
        {
            
            return false;
        }
    }


    public async Task<bool> UpdateAsync(int employeeId, string firstName, string lastName, int departmentId, decimal salary,string Email, string Password, string UserName)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeId);
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@DepartmentID", departmentId);
            parameters.Add("@Salary", salary);
            parameters.Add("@Email", Email);
            parameters.Add ("Password", Password);
            parameters.Add("@UserName", UserName);

            await _dbConnection.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure);

            return true;
        }
        catch (Exception ex)
        {
            
            return false;
        }
    }

   public async Task<bool> DeleteAsync(int employeeId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeId);

            await _dbConnection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

            return true;
        }
        catch (Exception ex)
        {
            
            return false;
        }
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", id);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<Employee>("GetEmployee", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }
        catch (Exception ex)
        {
            
            return null;
        }
    }


}

