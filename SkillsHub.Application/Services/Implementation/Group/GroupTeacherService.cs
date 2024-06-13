using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SkillsHub.Application.Services.Implementation;

public class GroupTeacherService : AbstractLogModelService<GroupTeacher> 
{
    public GroupTeacherService(ApplicationDbContext context)
    {
        _context = context;
        _contextModel = _context.GroupTeachers;
    }

    /*
    public override async Task<GroupTeacher> GetAsync(Guid? id)
    {
        var GroupTeacher = await base.GetAsync(id);
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == GroupTeacher.LessonTypeId);
        GroupTeacher.LessonType = lessonType;
        return GroupTeacher;

    }
    */
    public override async Task<bool> IsHardDelete(IQueryable<GroupTeacher> items)
    {
        return false;
    }
    

    /*
    public override async Task Validate(GroupTeacher oldValue, GroupTeacher newItem)
    {
        try
        {
            await base.Validate(oldValue,newItem);
        }
        catch (Exception ex) { if (ex.Message != "No changes") throw new Exception(ex.Message); }
    } */

}
