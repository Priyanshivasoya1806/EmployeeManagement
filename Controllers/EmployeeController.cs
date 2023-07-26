using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Serialization;
using Task3.Helper;
using Task3.Models;
using Task3.Repository;
using System;
using System.Threading.Tasks;


namespace Task3.Controllers
{
    [CustomAuthorization]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;


        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;

        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadEmployees(int startIndex = 0, int pageSize = 5, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            var totalRecords = await _repository.GetTotalRecords(searchValue);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var employees = await _repository.GetAllEmployee(startIndex, pageSize, searchValue, sortColumn, sortDirection);


            var viewModel = new DataTableParameters
            {
                StartIndex = startIndex,
                PageSize = pageSize,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                Employees = (List<Employee>)employees,
                TotalRecords = totalRecords,
                CurrentPage = (startIndex / pageSize) + 1,
                TotalPages = totalPages
            };
            
            return Json(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool created = await _repository.AddAsync(employee.FirstName, employee.LastName, employee.DepartmentID, employee.Salary, employee.Email, employee.Password, employee.UserName);

                if (created)
                {
                    TempData["success"] = "Employee Deleted successfully";
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create the employee.");
                }
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool updated = await _repository.UpdateAsync(employee.EmployeeID, employee.FirstName, employee.LastName, employee.DepartmentID, employee.Salary, employee.Email, employee.Password, employee.UserName);

                if (updated)
                {
                    TempData["success"] = "Employee Updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update the employee.");
                }
            }

            return View(employee);
        }


        //[HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _repository.DeleteAsync(id);   

            if (deleted)
            {
                TempData["success"] = "Employee Deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();  
            }
        }

        
    }

}
