using System.Security.Claims;

namespace web.Utils
{
    public class AuthHttpClient : HttpClient
    {
        public AuthHttpClient(ClaimsPrincipal user)
            : base()
        {
            var token = user.FindFirstValue("token");
            if (token != null)
            {
                base.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }
    }
}
