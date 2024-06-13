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

public class TeacherService : IUserRoleModelService<Teacher>
{
    private readonly ApplicationDbContext _context;
    private readonly IAbstractLogModel<LessonTypeTeacher> _lessonTypeTeacherService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeacherService(ApplicationDbContext context, IAbstractLogModel<LessonTypeTeacher> lessonTypeTeacherService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _lessonTypeTeacherService = lessonTypeTeacherService as LessonTypeTeacherService;
        _userManager = userManager;
    }

    public async Task<IQueryable<Teacher>> GetAllAsync()
    {
        var items = _context.Teachers
            .Include(x => x.ApplicationUser).ThenInclude(x => x.ConnectedUsersInfo).ThenInclude(x=>x.BaseUserInfo)
            .Include(x => x.PossibleCources)
            .AsQueryable();
        foreach (var item in items)
        {
            var dbLessonTypesStudentIds = item.PossibleCources?.Select(x => x.Id);
            List<LessonTypeTeacher> dbLessonTypesStudens = new List<LessonTypeTeacher>();
            foreach (var lessonTypeId in dbLessonTypesStudentIds)
            {
                var lessonTypeStudent = await _lessonTypeTeacherService.GetAsync(lessonTypeId);
                dbLessonTypesStudens.Add(lessonTypeStudent);
            }
            item.PossibleCources = dbLessonTypesStudens;//.Where(x => !x.IsDeleted && x.ParentId == null).ToList();
        }

        return items;
    }

    public async Task<Teacher> GetAsync(Guid teacherId)
    {
        var item = await _context.Teachers.Include(x => x.ApplicationUser).ThenInclude(x => x.ConnectedUsersInfo).ThenInclude(x => x.BaseUserInfo).Include(x=>x.PossibleCources).FirstOrDefaultAsync(x=>x.Id == teacherId);
          
        var dbLessonTypesTeacherIds = item.PossibleCources?.Select(x=>x.Id);
        List<LessonTypeTeacher> dbLessonTypesStudens = new List<LessonTypeTeacher>();
        foreach(var lessonTypeId in  dbLessonTypesTeacherIds)
        {
            var lessonTypeTeacher = await _lessonTypeTeacherService.GetAsync(lessonTypeId);
            dbLessonTypesStudens.Add(lessonTypeTeacher);
        }
        item.PossibleCources = dbLessonTypesStudens;//.Where(x => !x.IsDeleted && x.ParentId == null).ToList();
        return item;
    }
    public async Task<Teacher> UpdateAsync(Teacher teacher, Guid[] lessonTypeId)
    {
        var dbTeacher = await _context.Teachers.Include(x=>x.PossibleCources).Include(x=>x.ApplicationUser).AsQueryable().FirstOrDefaultAsync(x=>x.Id == teacher.Id);
        if (teacher == null || dbTeacher == null) throw new Exception("Teacher not found");
        lessonTypeId.ToList().ForEach(x => CheckCorrectProperties(x, null));
        if (dbTeacher.IsDeleted != teacher.IsDeleted)
        {
            _context.Teachers.Update(teacher);
            try
            {
                await _userManager.UpdateSecurityStampAsync(dbTeacher.ApplicationUser);
                if (!teacher.IsDeleted) await _userManager.AddToRoleAsync(dbTeacher.ApplicationUser, "Teacher");
                else await _userManager.RemoveFromRoleAsync(dbTeacher.ApplicationUser, "Teacher");
            }
            catch (Exception ex) { }
            await _context.SaveChangesAsync();

        }
        

        var dbActiveLessonTypesTeacher = dbTeacher.CurrentPossibleCourses?.Select(x=>x.LessonTypeId).Distinct().ToArray() ?? new Guid[0];
        var dbNotActiveLessonTypesTeacher = dbTeacher?.PossibleCources?.Where(x => x.ParentId == null && x.IsDeleted).Select(x => x.LessonTypeId).ToArray() ?? new Guid[0];
        var allEqual = dbActiveLessonTypesTeacher.SequenceEqual(lessonTypeId);
        if (allEqual) return teacher;

        var toDelete = dbActiveLessonTypesTeacher.Except(lessonTypeId).ToArray();  //there is in dbActive... but no in lessonTypeId []
        var toUpdate = dbNotActiveLessonTypesTeacher.Intersect(lessonTypeId).ToArray(); //there is in dbNotActive but no in lessonTypeId []
        var toCreate = lessonTypeId.Except(dbActiveLessonTypesTeacher).Except(dbNotActiveLessonTypesTeacher).ToArray(); //no in dbActive and dbNotActive but in lessonTypeId []

        var possibleIds = toCreate.Union(toUpdate).ToList();
        var possible = possibleIds.Select(x => new LessonTypeTeacher() { TeacherId = teacher.Id, LessonTypeId = x }).ToList();

        foreach (var LessTypeId in toCreate)
        {
            var item = new LessonTypeTeacher() { TeacherId = teacher.Id, LessonTypeId = LessTypeId , IsDeleted = false};
            try
            {
                await _lessonTypeTeacherService.CreateAsync(item);
            }
            catch (Exception ex)
            {
                await _lessonTypeTeacherService.UpdateAsync(item);
            }
            
        }
        foreach (var LessTypeId in toDelete)
        {

            var item = await _context.LessonTypeTeachers.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).FirstOrDefaultAsync(x => x.TeacherId == teacher.Id && x.LessonTypeId == LessTypeId);
            await _lessonTypeTeacherService.RemoveAsync(item.Id);
        }
        foreach(var LessTypeId in toUpdate)
        {
            var item = await _context.LessonTypeTeachers.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).FirstOrDefaultAsync(x => x.TeacherId == teacher.Id && x.LessonTypeId == LessTypeId);
            if (lessonTypeId.Contains(LessTypeId)) item.IsDeleted = false;
            await _lessonTypeTeacherService.UpdateAsync(item);
        }
        await _context.SaveChangesAsync();
        teacher.PossibleCources = possible;
        return teacher;
    }

    public async Task<Teacher> CreateAsync(Teacher teacher, Guid[] lessonTypeId)
    {
        var dbTeacher = await _context.Teachers.FindAsync(teacher.Id);
        var dbTeacher2 = await _context.Teachers.FirstOrDefaultAsync(x => x.ApplicationUserId == teacher.ApplicationUserId);
        if (teacher == null || dbTeacher != null || dbTeacher2 != null) throw new Exception("Teacher already exist");
        lessonTypeId.ToList().ForEach(x => CheckCorrectProperties(x, null));
        var resTeacher = await _context.Teachers.AddAsync(teacher);
        teacher = resTeacher.Entity;
        await _context.SaveChangesAsync();

        var dbLessonTypesTeacher = _context.LessonTypeTeachers.Where(x => x.TeacherId != null && x.TeacherId == teacher.Id && x.LessonTypeId != null).Select(x => x.LessonTypeId).ToArray();
        var toCreate = lessonTypeId.Except(dbLessonTypesTeacher);

        var possible = lessonTypeId.Select(x => new LessonTypeTeacher() { TeacherId = teacher.Id, LessonTypeId = x }).ToList();
        foreach (var item in possible)
        {
            await _lessonTypeTeacherService.CreateAsync(item);
        }
        await _context.SaveChangesAsync();
        teacher.PossibleCources = possible;

        var user = await _context.ApplicationUsers.FindAsync(teacher.ApplicationUserId);
        await _userManager.UpdateSecurityStampAsync(user);
        var result = await _userManager.AddToRoleAsync(user, "Teacher");
        if (!result.Succeeded) throw new Exception(result.Errors.ToString());
        await _context.SaveChangesAsync();

        return dbTeacher ?? throw new Exception("Error with save teacher in database");
    }

    public void CheckCorrectProperties(Guid lessonTypeId, Guid? teacherId)
    {
        var isFound = _context.LessonTypes.FirstOrDefault(x => x.Id == lessonTypeId && !x.IsDeleted) != null ? true : throw new Exception("Now lesson type is deleted, not active or not found");
        if (teacherId == null || teacherId == Guid.Empty) return;
        isFound = _context.Teachers.FirstOrDefault(x => x.Id == teacherId && !x.IsDeleted) != null ? true : throw new Exception("Now teacher is not active or not found");
    }


}
