using Microsoft.EntityFrameworkCore;
using Your_Finance_Escort.Services.AuthAPI.Data;
using Your_Finance_Escort.Services.AuthAPI.Models.Dto;
using Your_Finance_Escort.Services.AuthAPI.Service.IService;

namespace Your_Finance_Escort.Services.AuthAPI.Service
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> GetUsersByRole(string role)
        {
            var users = await _context.Roles
                .Where(u => u.Name == role) // Filtering by role
                .ToListAsync();

            var employeeDtos = users.Select(u => new CustomerDto
            {

                Name = u.Name,

            }).ToList();

            return employeeDtos;
        }
    }
}
