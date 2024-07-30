using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mangoes.Services.CartAPI.Utilities
{
    public class BackendApiDelegatingHandler:DelegatingHandler
    {
        private IHttpContextAccessor httpContextAccessor;
        public BackendApiDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
