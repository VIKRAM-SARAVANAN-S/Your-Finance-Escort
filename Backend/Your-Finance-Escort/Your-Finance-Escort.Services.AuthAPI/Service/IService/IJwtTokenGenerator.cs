using Your_Finance_Escort.Services.AuthAPI.Models;

namespace Your_Finance_Escort.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(ApplicationUser applicationUser);
    }
}
