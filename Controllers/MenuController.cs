using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Task3.Models;
using Task3.Repository;

namespace Task3.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuItemRepository _menuRepository;
        private readonly IConfiguration _configuration;

        public MenuController(IMenuItemRepository menuRepository, IConfiguration configuration)
        {
            _menuRepository = menuRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (authToken != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(authToken);
                var userRole = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                string roleName = userRole;

                var menuItems = _menuRepository.GetMenuItemsByRole(roleName);

                var viewModel = new MenuViewModel
                {
                    MenuItems = menuItems,
                    Role = roleName
                };

                return PartialView("_MenuPartialView", viewModel);
            }

            return RedirectToAction("Login");
        }


    }
}
