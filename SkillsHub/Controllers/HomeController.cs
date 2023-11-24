using AutoMapper;
using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Models;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IExternalService _externalService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IMailService mailService, IExternalService externalService, SignInManager<ApplicationUser> signInManager)
        {
            _mailService = mailService;
            _externalService = externalService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","CRM");
            }
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }


        [HttpPost]

        public async Task<ViewResult> SendMessage(SendingMessage msg)
        {
            #region ViberMessage
            /*
var _mailer = new ViberMailer.Mailer();
_mailer.Init();
await _mailer.SendMessage();
*/
            #endregion

            #region Own Viber Message

            /*
            var jsonRequest =
            "{\"receiver\":\"HieQ+DqPhvuugpuSMep9zg==\"," +
            "\"sender\":{\"name\":\"SkillsHub\"},\"type\":\"text\",\"text\":\"Hello world!\"," +
                "\"auth_token\":\"5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f\"}";
            var url = "https://chatapi.viber.com/pa/send_message";

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                //var res2 = await httpClient.PostAsync("https://chatapi.viber.com/pa/send_message")

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {jsonResponse}");
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            */

            #endregion

            //if (!ModelState.IsValid) return View("Index",msg);
            
            await _mailService.SendEmailAsync(msg);
            var message = new SkillsHub.Domain.Models.EmailMessage() { Data = msg.Data, Email = msg.Email, Date = msg.Date, Name = msg.Name, Phone = msg.Phone };

            await _externalService.SaveMessage(message);


            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}