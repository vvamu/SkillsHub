using Microsoft.AspNetCore.Mvc;
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
        if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");
        if (cource == null) throw new Exception("Cource not found");
        if (lessonType == null) throw new Exception("LessonType not found");

        var groupDb = new Group() { Name = item.Name, CourceName = cource , LessonType = lessonType };
        await _context.Groups.AddAsync(groupDb);
        await _context.SaveChangesAsync();
        var groupResult = await _context.Groups.FirstOrDefaultAsync(x=>x.Name == item.Name) ?? throw new Exception("Group not created");


        var result = await AddTeacherToGroupAsync(groupResult.Id, item.TeacherId);

        return result;

    }

    public async Task<Group> AddStudentsToGroupAsync(Guid groupId, List<Guid> studentsId)
    {

        var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
        if (studentsId.Count() != 1 && group == null) throw new Exception("Group not found. Maybe group is created but students were not add"); //Group classes

        List <LessonStudent> studentsList = new List<LessonStudent>();
        foreach (var studentId in studentsId)
        {
            var dbItem = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId) ?? throw new Exception("Student not found");
            studentsList.Add(new LessonStudent() { Student = dbItem });
        }
        await _context.LessonStudents.AddRangeAsync(studentsList);
        await _context.SaveChangesAsync();
        group.ArrivedStudents = studentsList;
        
        _context.Update(group);
        await _context.SaveChangesAsync();
        return group;

    }

    public async Task<Group> AddTeacherToGroupAsync(Guid groupId, Guid teacherId)
    {
        var group = await _context.Groups.Include(x=>x.Teacher).FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new Exception("Group not found. Maybe group is created but students were not add");
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == teacherId) ?? throw new Exception("Teacher not found in database");
        //if (group.Teacher != null) throw new Exception("Group already have teacher");
        group.Teacher = teacher;
        teacher.Groups = new List<Group> { group };
        _context.Update(group);
        _context.Update(teacher);
        await _context.SaveChangesAsync();

        return group;

    }

    public IQueryable<Group> GetAll() => _context.Groups.Include(x=>x.Lessons).Include(x=>x.ArrivedStudents).AsQueryable();

    public IQueryable<Group> GetFree() => _context.Groups.Where(x=>x.Teacher == null).Include(x => x.Lessons).Include(x => x.ArrivedStudents).AsQueryable();
}
