using Microsoft.AspNetCore.Identity;

namespace Your_Finance_Escort.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
