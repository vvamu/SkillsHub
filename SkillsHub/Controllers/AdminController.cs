using Microsoft.AspNetCore.Mvc;

namespace SkillsHub.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateUser() 
    {
        return View();
    }

    //[HttpPost]
    //public IActionResult CreateUser(UserDTO user)
    //{
    //    return View();
    //}

}
