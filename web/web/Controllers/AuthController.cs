using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
