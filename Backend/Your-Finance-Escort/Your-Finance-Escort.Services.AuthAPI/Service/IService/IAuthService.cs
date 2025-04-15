using Your_Finance_Escort.Services.AuthAPI.Models.Dto;

namespace Your_Finance_Escort.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string rolename);
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<List<UserDto>> GetAllUsersAsync(); // New method
        Task<bool> UpdateUserAsync(string userId, UserDto updatedUser);

    }
}
