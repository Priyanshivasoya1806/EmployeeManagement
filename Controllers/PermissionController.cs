using Microsoft.AspNetCore.Mvc;
using Task3.Models;
using Task3.Repository;

namespace Task3.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;

        }
        public ActionResult Index(int id)
        {
            
            var permissions = _permissionRepository.GetPermissions();
            ViewBag.Permissions = permissions;
            ViewBag.RoleID = id;
            return View();
        }

        [HttpPost]
        public ActionResult AssignPermissionsToRole(int roleId, int[] permissionIds)
        {
            _permissionRepository.AssignPermissionsToRole(roleId, permissionIds);
            TempData["success"] = "Role: " + roleId + " is successfully Assign the Permission ";
            return RedirectToAction("Index","Role");
        }
    }
}
