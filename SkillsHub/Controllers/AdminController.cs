using Microsoft.AspNetCore.Mvc;
using SkillsHub.Domain.DTO;

namespace SkillsHub.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CreateStudent() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateStudent(StudentDTO user)
    {
        return View();
    }

    //[HttpPost]
    //public IActionResult CreateUser(UserDTO user)
    //{
    //    return View();
    //}

}
