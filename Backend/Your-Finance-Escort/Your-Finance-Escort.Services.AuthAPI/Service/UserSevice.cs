using Microsoft.EntityFrameworkCore;
using Your_Finance_Escort.Services.AuthAPI.Data;
using Your_Finance_Escort.Services.AuthAPI.Models.Dto;
using Your_Finance_Escort.Services.AuthAPI.Service.IService;

namespace Your_Finance_Escort.Services.AuthAPI.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> GetUsersByRole(string role)
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);

                return usersInRole.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException($"Error retrieving users by role: {ex.Message}");
            }
        }


    }
}
