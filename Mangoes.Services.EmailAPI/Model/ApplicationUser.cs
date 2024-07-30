using Microsoft.AspNetCore.Identity;

namespace Mangoes.Services.EmailAPI.Model
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; } = " ";
    }
}
