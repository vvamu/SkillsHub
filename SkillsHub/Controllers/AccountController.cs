using Microsoft.AspNetCore.Mvc;
using Viber.Bot;
using Viber.Bot.NetCore.Models;

namespace SkillsHub.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        public AccountController() { }
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
