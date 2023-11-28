using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System.Collections.Generic;

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
        if (item == null) throw new Exception("Not correct data for group");
        var userRegisterValidator = new GroupValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        var cource = await _context.CourceNames.FirstOrDefaultAsync(x => x.Id == item.CourceId);
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == item.LessonTypeId);
        item.LessonType = lessonType;
        if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");
        if (cource == null) throw new Exception("Cource not found");
        if (lessonType == null) throw new Exception("LessonType not found");
        item.LessonType = lessonType;
        item.CourceName = cource;

        //var groupDb = new Group() { Name = item.Name, CourceName = cource , LessonType = lessonType };
        await _context.Groups.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;

    }

    public async Task<Group> AddStudentsToGroupAsync(Guid itemId, List<Guid> studentsId)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == itemId) ?? throw new Exception("Group not found. Maybe group is created but students were not add");
        List<Student> studentsList = new List<Student>();
        foreach (var studentId in studentsId)
        {
            var dbItem = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId) ?? throw new Exception("Student not found");
            studentsList.Add(dbItem);
        }
        group.ArrivedStudents = studentsList;
        
        _context.Update(group);
        await _context.SaveChangesAsync();
        return group;

    }



    public IQueryable<Group> GetAll() => _context.Groups.Include(x=>x.Lessons).Include(x=>x.ArrivedStudents).AsQueryable();

}
