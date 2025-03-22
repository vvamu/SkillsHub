using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class StudentService : IAbstractLogModelService<Student>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        //_lessonTypeStudentService = lessonTypeStudentService as LessonTypeStudentService;
        _userManager = userManager;
    }

    public async Task<Student> GetLastValueAsync(Guid? itemId, bool withParents = false, bool touchFullInclude = true)
    {
        var item = await _context.Students
            .Include(x => x.ApplicationUser)
            .ThenInclude(x => x.ConnectedUsersInfo)
            .ThenInclude(x => x.BaseUserInfo)
            .FirstOrDefaultAsync(x => x.Id == itemId);
        return item;
        //var dbLessonTypesStudentIds = item.Select(x => x.Id);
        //foreach (var lessonTypeId in dbLessonTypesStudentIds)
        //{
        //    //var lessonTypeStudent = await _lessonTypeStudentService.GetAsync(lessonTypeId);
        //    //dbLessonTypesStudens.Add(lessonTypeStudent);
        //}
        //item.PossibleCources = dbLessonTypesStudens;//.Where(x => !x.IsDeleted && x.ParentId == null).ToList();

        //var dbLessonTypesStudentIds = item.Select(x => x.Id);
        //List<LessonTypeStudent> dbLessonTypesStudens = new List<LessonTypeStudent>();
        //foreach (var lessonTypeId in dbLessonTypesStudentIds)
        //{
        //    //var lessonTypeStudent = await _lessonTypeStudentService.GetAsync(lessonTypeId);
        //    //dbLessonTypesStudens.Add(lessonTypeStudent);
        //}
        //item.PossibleCources = dbLessonTypesStudens;//.Where(x => !x.IsDeleted && x.ParentId == null).ToList();

    }

    public IQueryable<Student> GetItems(bool onlyCurrent = false, bool withParents = false, bool touchFullInclude = false)
    {
        var items = _context.Students
           .Include(x => x.ApplicationUser).ThenInclude(x => x.ConnectedUsersInfo).ThenInclude(x => x.BaseUserInfo)
           //.Include(x => x.PossibleCources)
           .AsQueryable();
        return items;
    }


    public async Task<Student> UpdateAsync(Student student)//, Guid[] lessonTypeId)
    {
        var dbStudent = await _context.Students.Include(x => x.ApplicationUser).AsQueryable().FirstOrDefaultAsync(x => x.Id == student.Id);
        if (student == null || dbStudent == null) throw new Exception("Student not found");
        //lessonTypeId.ToList().ForEach(x => CheckCorrectProperties(x, null));
        if (dbStudent.IsDeleted != student.IsDeleted)
        {
            _context.Students.Update(student);
            await _userManager.UpdateSecurityStampAsync(dbStudent.ApplicationUser);
            if (!student.IsDeleted) await _userManager.AddToRoleAsync(dbStudent.ApplicationUser, "Student");
            else await _userManager.RemoveFromRoleAsync(dbStudent.ApplicationUser, "Student");
            await _context.SaveChangesAsync();

        }

        //var dbActiveLessonTypesStudent = dbStudent.CurrentPossibleCourses?.Select(x => x.LessonTypeId).Distinct().ToArray() ?? new Guid[0];
        //var dbNotActiveLessonTypesStudent = dbStudent?.PossibleCources?.Where(x => x.ParentId == null && x.IsDeleted).Select(x => x.LessonTypeId).ToArray() ?? new Guid[0];
        //var allEqual = dbActiveLessonTypesStudent.SequenceEqual(lessonTypeId);
        //if (allEqual) return student;

        //var toDelete = dbActiveLessonTypesStudent.Except(lessonTypeId).ToArray();  //there is in dbActive... but no in lessonTypeId []
        //var toUpdate = dbNotActiveLessonTypesStudent.Intersect(lessonTypeId).ToArray(); //there is in dbNotActive but no in lessonTypeId []
        //var toCreate = lessonTypeId.Except(dbActiveLessonTypesStudent).Except(dbNotActiveLessonTypesStudent).ToArray(); //no in dbActive and dbNotActive but in lessonTypeId []

        //var possibleIds = toCreate.Union(toUpdate).ToList();
        //var possible = possibleIds.Select(x => new LessonTypeStudent() { StudentId = student.Id, LessonTypeId = x }).ToList();

        //foreach (var LessTypeId in toCreate)
        //{
        //    var item = new LessonTypeStudent() { StudentId = student.Id, LessonTypeId = LessTypeId, IsDeleted = false };
        //    try
        //    {
        //        await _lessonTypeStudentService.CreateAsync(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _lessonTypeStudentService.UpdateAsync(item);
        //    }

        //}
        //foreach (var LessTypeId in toDelete)
        //{

        //    var item = await _context.LessonTypeStudents.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).FirstOrDefaultAsync(x => x.StudentId == student.Id && x.LessonTypeId == LessTypeId);
        //    await _lessonTypeStudentService.RemoveAsync(item.Id);
        //}
        //foreach (var LessTypeId in toUpdate)
        //{
        //    var item = await _context.LessonTypeStudents.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).FirstOrDefaultAsync(x => x.StudentId == student.Id && x.LessonTypeId == LessTypeId);
        //    if (lessonTypeId.Contains(LessTypeId)) item.IsDeleted = false;
        //    await _lessonTypeStudentService.UpdateAsync(item);
        //}
        await _context.SaveChangesAsync();
        //student.PossibleCources = possible;
        return student;


        /*
                if (!User.IsInRole("Admin"))
                {
                    var message = "User in student account " + User.Identity.Name + " tried to change payment amount ";
                    await _notificationService.Create(message,null);
                    await _userService.SignOutAsync();
                    throw new Exception("An attempt was made to fix a field that is only accessible to the administrator. The request has been sent to the appropriate place");

                    //return RedirectToAction("Index", "Home");
                
                }
            */
    }

    public async Task<Student> CreateAsync(Student student)//, Guid[] lessonTypeId)
    {
        var dbStudent = await _context.Students.FindAsync(student.Id);
        if (student == null || dbStudent != null) throw new Exception("Student already exist");
        //lessonTypeId.ToList().ForEach(x => CheckCorrectProperties(x, null));
        var resStudent = await _context.Students.AddAsync(student);
        student = resStudent.Entity;
        await _context.SaveChangesAsync();

        var dbLessonTypesStudent = _context.LessonTypeStudents.Where(x => x.StudentId != null && x.StudentId == student.Id && x.LessonTypeId != null).Select(x => x.LessonTypeId).ToArray();
        //var toCreate = lessonTypeId.Except(dbLessonTypesStudent);

        //var possible = lessonTypeId.Select(x => new LessonTypeStudent() { StudentId = student.Id, LessonTypeId = x }).ToList();
        //foreach (var item in possible)
        //{
        //    await _lessonTypeStudentService.CreateAsync(item);
        //}
        //await _context.SaveChangesAsync();
        //student.PossibleCources = possible;

        var user = await _context.Users.FindAsync(student.ApplicationUserId);
        try
        {
            await _userManager.UpdateSecurityStampAsync(user);
            var result = await _userManager.AddToRoleAsync(user, "Student");
            if (!result.Succeeded) throw new Exception(result.Errors.ToString());
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }


        return student ?? throw new Exception("Error with save teacher in database");
    }

    public void CheckCorrectProperties(Guid lessonTypeId, Guid? studentId)
    {
        var isFound = _context.LessonTypes.FirstOrDefault(x => x.Id == lessonTypeId && !x.IsDeleted) != null ? true : throw new Exception("Now lesson type is deleted, not active or not found");
        if (studentId == null || studentId == Guid.Empty) return;
        isFound = _context.Students.FirstOrDefault(x => x.Id == studentId && !x.IsDeleted) != null ? true : throw new Exception("Now student is not active or not found");
    }

    

    public Task<Student> RemoveAsync(Guid itemId)
    {
        throw new NotImplementedException();
    }

    public Task<Student> RestoreAsync(Guid itemId)
    {
        throw new NotImplementedException();
    }
}
