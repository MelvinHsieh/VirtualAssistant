using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using web.Models;
using web.Utils;

namespace web.Controllers
{
    public class AuthController : Controller
    {
        private readonly string _authURL;

        public AuthController(IConfiguration configuration)
        {
            _authURL = configuration.GetValue<String>("AuthURL");
        }

        [HttpGet("Account/Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("Account/Login")]
        [AllowAnonymous]
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

        [HttpPost()]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async void Authenticate (string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin1"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("token", token)
            };

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
