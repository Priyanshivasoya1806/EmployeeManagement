using Microsoft.AspNetCore.Mvc;
using System.Data;
using Task3.Helper;
using Task3.Models;
using Task3.Repository;

namespace Task3.Controllers
{
    [CustomAuthorization]

    public class RoleController : Controller
    {
        private readonly IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        // GET: Role
        public ActionResult Index()
        {
            var roles = _repository.GetRoles();
            return View(roles);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Role Created successfully";
                _repository.CreateRole(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            var role = _repository.GetRoleById(id);
            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Role Updated successfully";
                _repository.UpdateRole(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            var role = _repository.GetRoleById(id);
            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["success"] = "Role Deleted successfully";
            _repository.DeleteRole(id);
            return RedirectToAction("Index");
        }

           
    }
}
