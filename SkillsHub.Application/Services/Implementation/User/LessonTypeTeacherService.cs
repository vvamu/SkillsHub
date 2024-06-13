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

public class LessonTypeTeacherService : AbstractLogModelService<LessonTypeTeacher> 
{
    public LessonTypeTeacherService(ApplicationDbContext context)
    {
        _context = context;
        _contextModel = _context.LessonTypeTeachers;
    }

    public override async Task<LessonTypeTeacher> GetAsync(Guid? id, bool withParents = false)
    {
        var LessonTypeTeacher = await base.GetAsync(id);
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == LessonTypeTeacher.LessonTypeId);
        LessonTypeTeacher.LessonType = lessonType;
        return LessonTypeTeacher;

    }
    public override async Task<bool> IsHardDelete(IQueryable<LessonTypeTeacher> items)
    {
        return false;
    }

    public override async Task Validate(LessonTypeTeacher oldValue, LessonTypeTeacher newItem)
    {
        try
        {
            await base.Validate(oldValue,newItem);
        }
        catch (Exception ex) { if (ex.Message != "No changes") throw new Exception(ex.Message); }
    } 

}
