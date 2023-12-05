using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Persistence;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SkillsHub.Controllers;

public class LessonController : Controller
{
    private readonly ApplicationDbContext _context;

    public LessonController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var lessons = await _context.Lessons.ToListAsync();
        return View(lessons);
    }

    [HttpGet]
    public async Task<IActionResult> IndexByGroup(Guid id)
    {
        var items = await _context.Lessons.Where(x => x.Group.Id == id).ToListAsync();

        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        var group = await _context.Groups.FindAsync(id);
        var item = new Lesson() { Group = group };
        return View(item);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
        return View(lesson);
    }
    [HttpPut]
    public async Task<IActionResult> Edit(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
        return View(lesson);
    }
}
