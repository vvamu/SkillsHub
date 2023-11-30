using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class ClassesService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public ClassesService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<Lesson>> GetClassesByTeacherAsync(Guid id)
    {
        var user = await _context.Teachers.FindAsync(id);
        if (user == null) throw new MissingMemberException();
        // var classes = await _context.Lessons.Include(x => x.Teachers).Select(x => x.Teachers).SingleAsync();
        //var .Where(x=>x.Teachers.Id == id).ToListAsync();
        var classes = await _context.Teachers.Include(x => x.Lessons).Select(x => x.Lessons).SingleAsync();
        return classes;
    }
    public async Task<List<Lesson>> GetClassesByStudentAsync(Guid id)
    {
        var user = await _context.Students.FindAsync(id);
        if (user == null) throw new MissingMemberException();
        var classes = await _context.Students.Include(x => x.Lessons).Select(x => x.Lessons).SingleAsync();

        return classes;
    }

    public async Task<Lesson> GetAsync(int id)
    {
        var item = await _context.Lessons.FindAsync(id);
        if (item == null) throw new ArgumentException();
        return item;
    }
    public async Task<Lesson> CreateAsync(Lesson item)
    {
        await _context.Lessons.AddAsync(item);
        await _context.SaveChangesAsync();
        //var result =await  _context.Lessons.Where(x=>x.StartTime  == item.StartTime).Where(x=>x.Teachers == item.Teacher).FirstOrDefaultAsync();
        //var result = await _context.Lessons.Where(x=>x)

        return item == null ? throw new ApplicationException() : item;
    }
    public async Task<Lesson> DeleteAsync(int id)
    {
        var item = await _context.Lessons.FindAsync(id);
        if (item == null) throw new ArgumentException();
        _context.Lessons.Remove(item);
        await _context.SaveChangesAsync();
        return item;
    }

}
