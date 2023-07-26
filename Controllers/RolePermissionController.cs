using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task3.Repository;

namespace Task3.Controllers
{
    public class RolePermissionController : Controller
    {
        private readonly IRolePermissionRepository _repository;
    
        public RolePermissionController(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(int id)
        {
            var roles = _repository.GetRoles();

            ViewBag.Roles = roles;
            ViewBag.EmployeeID = id;

            return View();
        }

        [HttpPost]
        public ActionResult AssignRolesToUser(int EmployeeID, int[] roleIds)
        {
            _repository.AssignRolesToUser(EmployeeID, roleIds);
            TempData["success"] = EmployeeID +" " + "is successfully Assign the role";
            return RedirectToAction("Index","Employee");
        }

        
    }
}
