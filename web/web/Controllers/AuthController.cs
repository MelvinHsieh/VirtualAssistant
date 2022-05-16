using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using web.Models;

namespace web.Controllers
{
    public class AuthController : Controller
    {
        private readonly string _authURL;
        public AuthController(IConfiguration configuration)
        {
            _authURL = configuration.GetValue<String>("AuthURL");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_authURL + "/login");
                try
                {
                    var response = client.PostAsJsonAsync(uri, model).Result;

                    if(response.IsSuccessStatusCode) { 

                        string result = await response.Content.ReadAsStringAsync();
                        string? jwt = JsonConvert.DeserializeObject<string>(result);

                        if(!string.IsNullOrEmpty(jwt))
                        {
                            Authenticate(jwt);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    TempData["error"] = "De login was niet succesvol!";
                    return View();
                }
            }
        }

        private void Authenticate (string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin1"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            HttpContext.SignInAsync("Cookies", principal);
        }
    }
}
