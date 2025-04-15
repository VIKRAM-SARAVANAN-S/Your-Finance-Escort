
using Microsoft.AspNetCore.Identity;
using Your_Finance_Escort.Services.AuthAPI.Data;
using Your_Finance_Escort.Services.AuthAPI.Models.Dto;
using Your_Finance_Escort.Services.AuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using Your_Finance_Escort.Services.AuthAPI.Service.IService;

namespace Your_Finance_Escort.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ResponseDto _response = new ResponseDto();

        public AuthService(AppDbContext dbContext, UserManager<ApplicationUser> userManager,
                           IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }

        public async Task<string> Register(RegistrationRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                bool res = await AssignRole(request.Email, request.Role);
                if (res == true)
                {
                    return null;
                }
                else
                {
                    return "error assigning role";
                }
            }
            return result.Errors.FirstOrDefault()?.Description;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new LoginResponseDto { User = null, Token = "", Role = "" };
            }

            // Generate the token and await the result
            var token = await _jwtTokenGenerator.GenerateToken(user); // Make sure to await this

            // Retrieve user role
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // Assuming single role per user

            // Populate UserDto with user's details
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            // Return the response with populated UserDto, Token, and Role
            return new LoginResponseDto
            {
                User = userDto,
                Token = token,
                Role = role
            };
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.Message = "User not found";
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    _response.Message = "Role creation failed";
                    return false;
                }
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleAssignmentResult.Succeeded)
            {
                _response.Message = "Role assignment to user failed";
                return false;
            }

            return true;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            }).ToList();

            return userDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            // Assume _userManager is a dependency injected UserManager<User> for user management
            var user = await _userManager.FindByEmailAsync(userId);
            var User = new UserDto
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name

            };

            return User;
        }

        public async Task<bool> UpdateUserAsync(string userId, UserDto updatedUser)
        {
            // Assume _userManager is injected UserManager<ApplicationUser>
            ApplicationUser user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Update user properties
            user.Email = userId;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Name = updatedUser.Name;

            // Save the changes
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
    }
