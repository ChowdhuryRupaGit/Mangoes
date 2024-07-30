using Mangoes.Services.AuthAPI.Model;

namespace Mangoes.Services.AuthAPI.Services.IServices
{
    public interface IJwtGenerator
    {
        string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
