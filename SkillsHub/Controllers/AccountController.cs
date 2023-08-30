using Microsoft.AspNetCore.Mvc;

namespace SkillsHub.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
