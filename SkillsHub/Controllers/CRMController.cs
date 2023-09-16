using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillsHub.Models;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers
{
    public class CRMController : Controller
    {
        private IMailService _mailService;
        public CRMController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}