using NuGet.Common;
using WebAppMango.Utilities;

namespace WebAppMango.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;

        }
        public void ClearToken()
        {
            _httpContextAccessor?.HttpContext?.Response.Cookies.Delete(SD.Token);
        }

        public string? GetToken()
        {
           return SD.Token;

           //bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.Token, out token);
          // return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            try
            {
               // _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.Token, token);
                SD.Token = token;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
