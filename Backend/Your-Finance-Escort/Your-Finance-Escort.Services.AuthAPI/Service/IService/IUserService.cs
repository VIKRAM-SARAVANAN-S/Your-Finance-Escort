using Your_Finance_Escort.Services.AuthAPI.Models.Dto;

namespace Your_Finance_Escort.Services.AuthAPI.Service.IService
{
    public interface IUserService
    {
        Task<List<CustomerDto>> GetUsersByRole(string role);

    }
}
