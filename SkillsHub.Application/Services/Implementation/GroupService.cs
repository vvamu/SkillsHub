using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class GroupService: IGroupService
{
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Group> CreateAsync(Group item)
    {
        //group validator
        await _context.Groups.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;

    }

    public IQueryable<Group> GetAll() => _context.Groups.Include(x=>x.Lessons).Include(x=>x.ArrivedStudents).AsQueryable();

}
