using Azure.Core;
using Azure;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task3.Models;
using Task3.Repository;

namespace Task3.Controllers
{

    public class SecurityController : Controller
    {
        private readonly IDbConnection _dbConnection;
        private IConfiguration Configuration { get; }
        private readonly IRoleRepository _roleRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public SecurityController(IDbConnection dbConnection, IConfiguration configuration, IRoleRepository roleRepository, IHttpContextAccessor httpContextAccessor   )
        {
            _dbConnection = dbConnection;
            Configuration = configuration;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateJSONWebToken(string username, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("Username and role cannot be null or empty.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHereYourSecretKeyHere"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
   {
        new Claim(ClaimTypes.Name, username), // Use ClaimTypes.Name for the unique name claim
        new Claim(ClaimTypes.Role, role),
        new Claim("Issuer", "Priyanshi"),
        new Claim("Admin", "true")
    };

            var token = new JwtSecurityToken(
         issuer: "Priyanshi",
         audience: "Priyanshi",
         claims: claims,
         expires: DateTime.Now.AddMinutes(120),
         signingCredentials: credentials
     );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //GET:api/Security
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //Get: api/Security/5
        //[HttpGet]
        //public string Get() 
        //{
        //    return GenerateJSONWebToken("Priyanshi@123");
        //}

        ////POST: api/Security
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{

        //}



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Uname", loginModel.Username);

            if (ModelState.IsValid)
            {
                string connectionString = Configuration.GetConnectionString("EmployeeManagement");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var user = connection.QueryFirstOrDefault<Employee>("GetEusername", parameters, commandType: CommandType.StoredProcedure);

                    if (user != null)
                    {
                        if (user.Password == loginModel.Password)
                        {
                            // Set a flag in TempData to indicate successful login
                            var roles = GetUserRoles(user.EmployeeID);
                            var role = roles.FirstOrDefault()?.RoleName;
                            var token = GenerateJSONWebToken(loginModel.Username, role);
                            TempData["success"] = loginModel.Username + " " +"is successfully logged in";
                            // Create and set the authentication cookie
                            if (token != null)
                            {
                                var cookieOptions = new CookieOptions
                                {

                                    Expires = DateTime.Now.AddSeconds(1000),
                                    HttpOnly = true, // Prevent JavaScript access to the cookie
                                    Secure = true, // Set to 'true' if using HTTPS
                                    SameSite = SameSiteMode.Strict
                                };
                                HttpContext.Response.Cookies.Append("authToken", token, cookieOptions);
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    return Unauthorized();
                }
            }

            return View("Login");
        }

        private IEnumerable<Role> GetUserRoles(int employeeId)
        {
            var userRoles = _roleRepository.GetUserRoles(employeeId);
            var roles = userRoles.Select(ur =>
            {
                var roleName = GetRoleName(ur.RoleID);
                return new Role { RoleID = ur.RoleID, RoleName = roleName };
            });
            return roles;
        }


        private string GetRoleName(int roleId)
        {
            string connectionString = Configuration.GetConnectionString("EmployeeManagement");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId);

                var roleName = connection.QuerySingleOrDefault<string>("GetRoleName", parameters, commandType: CommandType.StoredProcedure);
                return roleName ?? "Unknown Role";
            }
        }

        //if (loginModel.Username == "Priyanshi@gmail.com" && loginModel.Password == "Priyanshi@123")
        //{
        //    var token = GenerateJSONWebToken(loginModel.Username);
        //    return Ok(new { Token = token });

        //}

        //return Unauthorized();


        [HttpGet("Logout")]
        public async Task<IActionResult> Logout([FromQuery] string authToken)
        {
            // Clear the flag in TempData indicating that the user is logged in
            TempData.Clear();

            // Clear the authentication cookie
            HttpContext.Response.Cookies.Delete("authToken");

            // Redirect the user to the home page
            return Redirect("/");
        }
    }
}