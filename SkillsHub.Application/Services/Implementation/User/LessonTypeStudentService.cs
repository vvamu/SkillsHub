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

public class LessonTypeStudentService : AbstractLogModelService<LessonTypeStudent> 
{
    public LessonTypeStudentService(ApplicationDbContext context)
    {
        _context = context;
        _contextModel = _context.LessonTypeStudents;
    }

    public override async Task<LessonTypeStudent> GetAsync(Guid? id, bool withParents = false, bool touchFullInclude = true)
    {
        var lessonTypeStudent = await base.GetAsync(id);
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == lessonTypeStudent.LessonTypeId);
        lessonTypeStudent.LessonType = lessonType;
        return lessonTypeStudent;

    }
    public override async Task<bool> IsHardDelete(IQueryable<LessonTypeStudent> items)
    {
        /*
        foreach (var item in items) {
            
            var connectedLessonTypes = _context.LessonTypes.FirstOrDefaultAsync(x=>x.);
            if (connectedLessonTypes != null) return false;
    }*/
        return false;
    }

    public override async Task Validate(LessonTypeStudent oldValue, LessonTypeStudent newItem)
    {
        try
        {
            await base.Validate(oldValue,newItem);
        }
        catch (Exception ex) { if (ex.Message != "No changes") throw new Exception(ex.Message); }
    } 

}
