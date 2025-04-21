
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

        public async Task<ResponseDto> Register(RegistrationRequestDto request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResponseDto { IsSuccess = false, Message = "Email and password are required" };
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    EmailConfirmed = true // For demo purposes
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                // Assign role
                if (!string.IsNullOrWhiteSpace(request.Role))
                {
                    var roleResult = await AssignRole(user.Email, request.Role);
                    if (!roleResult)
                    {
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = "Role assignment failed"
                        };
                    }
                }

                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName) ??
                          await _userManager.FindByNameAsync(loginRequestDto.UserName);

                if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                {
                    return new LoginResponseDto
                    {
                        User = null,
                        Token = "",
                        Message = "Invalid credentials"
                    };
                }

                var token = await _jwtTokenGenerator.GenerateToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                return new LoginResponseDto
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    },
                    Token = token,
                    Role = roles.FirstOrDefault(),
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDto
                {
                    Message = $"Login failed: {ex.Message}"
                };
            }
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
