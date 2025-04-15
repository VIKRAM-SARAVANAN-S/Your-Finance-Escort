using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Finance_Escort.Services.AuthAPI.Models;

namespace Your_Finance_Escort.Services.AuthAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            // Ensure the Employee role exists
            var employeeRole = await _roleManager.FindByNameAsync("user");
            if (employeeRole == null)
            {
                return NotFound("Employee role not found.");
            }

            // Find users assigned the Employee role
            var employees = await _userManager.GetUsersInRoleAsync("user");

            // Map users to a response model or return directly
            var employeeDtos = employees.Select(e => new
            {
                e.Id,
                e.UserName,
                e.Email
            });

            return Ok(employeeDtos);
        }
    }
}
