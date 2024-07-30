using Microsoft.AspNetCore.Identity;

namespace Mangoes.Services.AuthAPI.Model
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; } = " ";
    }
}
