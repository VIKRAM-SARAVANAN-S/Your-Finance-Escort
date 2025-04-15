namespace Your_Finance_Escort.Services.AuthAPI.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
