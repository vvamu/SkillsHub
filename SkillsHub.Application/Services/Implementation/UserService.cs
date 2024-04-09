using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System.Linq;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Validators;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using Azure.Core;
using Spire.Xls.Charts;

namespace SkillsHub.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<ApplicationUser> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        //RoleManager<ApplicationUser> roleManager,
        ApplicationDbContext context, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        //_roleManager = roleManager;
        _context = context;
        _mapper = mapper;
    }
    #region Get

    public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
    {        
        var user = await _context.ApplicationUsers
            
            .Include(x => x.UserTeacher)

            .Include(x=>x.UserTeacher.PossibleCources).ThenInclude(x=>x.Course)
            .Include(x=>x.UserTeacher).ThenInclude(x=>x.GroupTeachers).ThenInclude(x=>x.Group)//.ThenInclude(x=>x.Student)
            .Include(x => x.UserTeacher).ThenInclude(x => x.GroupTeachers).ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher).ThenInclude(x => x.PossibleCources)
            .Include(x=>x.UserStudent)
            .Include(x=>x.UserStudent).ThenInclude(x=>x.Groups)//.ThenInclude(x=>x.Student)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups).ThenInclude(x => x.Group).ThenInclude(x=>x.Lessons)//.ThenInclude(x=>x.GroupStudents).ThenInclude(x => x.Student)
            .Include(x=>x.UserStudent)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.Course)

            .FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }

    public async Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id)
    {
        var user = await _context.ApplicationUsers.Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.Course)
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher.PossibleCources).ThenInclude(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return null;
        var userCreateDTO = _mapper.Map<UserCreateDTO>(user);
        if (user.UserTeacher != null) userCreateDTO.IsTeacher = true;
        if (user.UserStudent != null) userCreateDTO.IsStudent = true;

        return userCreateDTO;
    }
    public async Task<IQueryable<Student>> GetAllStudentsAsync()
    {
        var items = _context.Students
            .Include(x => x.ApplicationUser).ThenInclude(x => x.UserTeacher)
            .Include(x => x.Lessons).ThenInclude(x=>x.Lesson)
            .Include(x => x.PossibleCources)
            //.Include(x => x.WorkingDays)
            .Include(x => x.Groups).ThenInclude(x=>x.Group).ThenInclude(x=>x.Lessons)
            .OrderBy(on => on.Id);

        return items;
    }


    public IQueryable<Teacher> GetAllTeachers()
    {
        var items = _context.Teachers//.Include(x => x.Lessons)
            .Include(x => x.ApplicationUser)
            .Include(x => x.PossibleCources)
            //.Include(x => x.WorkingDays)
            .Include(x => x.GroupTeachers).OrderBy(on => on.Id);

        return items;

    }
    public async Task<IQueryable<ApplicationUser>> GetAllAsync()
    {
        return _context.Users
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher.PossibleCources).ThenInclude(x => x.Course)
            .Include(x => x.UserTeacher).ThenInclude(x => x.GroupTeachers)
            .Include(x => x.UserStudent)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.Course)
            .OrderBy(x => x.Id);
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userName = _signInManager.Context.User.Identity.Name;
        if (userName == null) throw new Exception();
        var dbUser = await _userManager.FindByNameAsync(userName);
        
        return await GetUserByIdAsync(dbUser.Id);

    }



    #endregion

    #region Create 
    public async Task<Teacher> CreateTeacherAsync(ApplicationUser user, Teacher item)
    {
        //var dbUser = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ?? throw new Exception("User not found");
        var dbUser = user;
        if (user == null) dbUser = item.ApplicationUser;
        var userRegisterValidator = new TeacherValidator();

        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        if(item.ApplicationUserId != Guid.Empty)
        {
            await _context.Teachers.AddAsync(item);
            await _context.SaveChangesAsync();

            var result = await _userManager.AddToRoleAsync(dbUser, "Teacher");
            if (!result.Succeeded) throw new Exception(result.Errors.ToString());
            await _context.SaveChangesAsync();

            return item == null ? throw new Exception("Error with save teacher in database") : item;
        }
        return item;

        /*
        item.ApplicationUser = dbUser;
        dbUser.UserTeacher = item;

        _context.Entry(item.ApplicationUser).State = EntityState.Unchanged;
        _context.Entry(dbUser.UserTeacher).State = EntityState.Unchanged;


        _context.ApplicationUsers.Update(dbUser);
        await _context.Teachers.AddAsync(item);

        
        //if (!result.Succeeded) throw new Exception(result.Errors.ToString());
        var teacherInDb = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == user.Id);


        return teacherInDb == null ? throw new Exception("Error with save teacher in database") : teacherInDb;
        */

    }
    public async Task<Student> CreateStudentAsync(ApplicationUser user, Student item)
    {
        //var dbUser = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ?? throw new Exception("User not found");
        var dbUser = user;
        if (dbUser == null) dbUser = item.ApplicationUser;
        if (dbUser == null) dbUser = await GetUserByIdAsync(item.ApplicationUserId);

        var userRegisterValidator = new StudentValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        if(item.ApplicationUserId != Guid.Empty)
        {
            await _context.Students.AddAsync(item);
            await _context.SaveChangesAsync();

            var res = await _userManager.AddToRoleAsync(dbUser, "Student");
            await _context.SaveChangesAsync();
            if (!res.Succeeded) throw new Exception(res.Errors.ToString());
            return item == null ? throw new CannotUnloadAppDomainException() : item;


        }


        ////---------------------------
        var student = _mapper.Map<Student>(item);
        student.ApplicationUser = dbUser;
       

        dbUser = item.ApplicationUser;

        dbUser.UserStudent = item;


        _context.Entry(student.ApplicationUser).State = EntityState.Unchanged;
        _context.Entry(dbUser.UserStudent).State = EntityState.Unchanged;

        _context.ApplicationUsers.Update(dbUser);
        await _context.Students.AddAsync(student);
        var result = await _userManager.AddToRoleAsync(dbUser, "Student");
        await _context.SaveChangesAsync();
        if (!result.Succeeded) throw new Exception(result.Errors.ToString());


        return student == null ? throw new CannotUnloadAppDomainException() : student;
    }
    public async Task<ApplicationUser> CreateUserAsync(UserCreateDTO item)
    {

        var user = _mapper.Map<ApplicationUser>(item);
        user.UserName = user.Login;
        var userRegisterValidator = new UserValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        if (_context.ApplicationUsers.FirstOrDefault(x => x.Login == user.Login) != null) throw new Exception("User with such login alredy exists");
        
        string hashedPassword = HashProvider.ComputeHash(user.Password.Trim());
        user.OwnHashedPassword = hashedPassword;
        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded) throw new Exception(string.Concat(result.Errors.Select(x=>x.Description)));

        await _context.SaveChangesAsync();

        _context.ApplicationUsers.Update(user);
        await _context.SaveChangesAsync();
        if (item.IsStudent == true) user.UserStudent = new Student() { ApplicationUser = user, IsDeleted = true };
        if (item.IsTeacher == true) user.UserTeacher = new Teacher() { ApplicationUser = user, IsDeleted = true };


        return user;

    }
    public async Task<ApplicationUser> UpdateUserAsync(UserCreateDTO item)
    {

        var user = _mapper.Map<ApplicationUser>(item);
        user.UserName = user.Login;
        var userRegisterValidator = new UserValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        if (user.OwnHashedPassword != HashProvider.ComputeHash(user.Password.Trim())) throw new Exception("Password not equal");

        user.Login = item.Login;
        var userInfo = new BaseUserInfo();
        //user = _mapper.Map<ApplicationUser>(item);
        userInfo.FirstName = item.FirstName;
        userInfo.LastName = item.LastName;
        userInfo.Email = item.Email;
        userInfo.Phone = item.Phone;
        userInfo.BirthDate = item.BirthDate;
        userInfo.Sex = item.Sex;
        user.UserInfo = userInfo;

        _context.BaseUserInfo.Update(userInfo);
        _context.ApplicationUsers.Update(user);

        await _context.SaveChangesAsync();


        if (item.IsStudent == true && user.UserStudent == null) user.UserStudent = new Student() { ApplicationUser = user, IsDeleted = true };
        if (item.IsTeacher == true && user.UserTeacher == null) user.UserTeacher = new Teacher() { ApplicationUser = user, IsDeleted = true };


        return user;

    }



    #endregion

    #region HZ - Update
    public async Task<Teacher> UpdateTeacherAsync(Teacher item)
    {
        //if (_context.Teachers.Any(x => x.Email == item.Email)) throw new Exception("Teacher with such email alredy exists");
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == item.Id) ?? throw new Exception("Teacher not found");
        var userRegisterValidator = new TeacherValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(teacher);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        _context.Teachers.Update(item);
        await _context.SaveChangesAsync();

        return item == null ? throw new CannotUnloadAppDomainException() : item;

    }

    #endregion

    #region Delete
    public async Task<Teacher> DeleteTeacherAsync(Guid id)
    {
        var dbItem = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
        var user = dbItem.ApplicationUser;
        dbItem.IsDeleted = true;
        await _userManager.RemoveFromRoleAsync(user, "Teacher");
        await _context.SaveChangesAsync();
        var teacher = _mapper.Map<Teacher>(dbItem);
        return teacher;
    }
    public async Task<Student> DeleteStudentAsync(Guid id)
    {
        var dbItem = await _context.Students.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
        var user = dbItem.ApplicationUser;
        dbItem.IsDeleted = true;
        await _userManager.RemoveFromRoleAsync(user, "Student");
        await _context.SaveChangesAsync();
        var student = _mapper.Map<Student>(dbItem);
        return student;
    }
    private async Task<ApplicationUser> DeleteUserAsync(Guid id)
    {
        var dbItem = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User not found");
        dbItem.IsDeleted = true;
        var teacher = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == id);
        var student = await _context.Students.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == id);
        if (teacher != null) teacher.IsDeleted = true;
        if (student != null) student.IsDeleted = true;

        await _userManager.UpdateAsync(dbItem);
        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(dbItem);
        foreach (var role in roles)
        {
            await _userManager.RemoveFromRoleAsync(dbItem, role);
        }
        await _context.SaveChangesAsync();

        return dbItem;
    }
    #endregion

    #region SignIn

    public async Task<ApplicationUser> SignInAsync(UserLoginDTO item)
    {
        var userRegisterValidator = new UserLoginValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        if (!_context.Users.Any(x => x.Login == item.Login)) throw new Exception("User with such login not exists");
        ApplicationUser? user = await _context.ApplicationUsers.Where(x => x.Login == item.Login).FirstOrDefaultAsync()
            ?? throw new Exception("User not found");

        var has1 = user.OwnHashedPassword;
        if (!HashProvider.VerifyHash(item.Password, has1)) throw new Exception("Incorrect password");

        await _signInManager.SignInAsync(user, true);

        return user;
    }
    public async Task<ApplicationUser> SignInAsync(ApplicationUser item)
    {
        var userRegisterValidator = new UserValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            //throw new Exception(errorsString);
        }

        ApplicationUser? user = await _context.ApplicationUsers.Where(x => x.Login == item.Login).FirstOrDefaultAsync()
            ?? throw new Exception("User not found");

        var has1 = user.OwnHashedPassword;
        if (!HashProvider.VerifyHash(item.Password, has1)) throw new Exception("Incorrect password");

        await _signInManager.SignInAsync(user, true);
        return user;
    }
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task InitialCreateAsync()
    {
        await CreateAdminAsync();
    }

    public async Task CreateAdminAsync()
    {
        var result = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Login == "AdminLogin");
        if (result != null) return;

        var adminI = new ApplicationUser()
        {
            Login = "AdminLogin",
            UserName = "AdminLogin",
            Password = "AdminPassword123",
            OwnHashedPassword = HashProvider.ComputeHash("AdminPassword123"),
            IsDeleted = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var admin = new BaseUserInfo()
        {
            
            FirstName = "AdminFirstName",
            LastName = "AdminLastName",
            Surname = "AdminSurname",
            Sex = "Male",
           
        };

        await _context.BaseUserInfo.AddAsync(admin);
        await _context.SaveChangesAsync();
        adminI.UserInfo = admin;

        await _context.ApplicationUsers.AddAsync(adminI);
        await _userManager.CreateAsync(adminI, adminI.Password.Trim());
        await _context.SaveChangesAsync();

       

        var re = await _userManager.AddToRoleAsync(adminI, "Admin");


        if (re.Succeeded)


            await _context.SaveChangesAsync();
    }



    #endregion

    /*
    public async Task<Teacher> UpdateTeacherWithCourcesNames(Teacher item, List<Guid> courcesId)
    {
        
        //VAR 2

        // Загрузите все существующие связи CourceNameTeacher для данного учителя
        var existingCourceNames = _context.LessonTypeTeachers
            .Where(ct => ct.TeacherId == item.Id)
            .ToList();

        // Удалите все существующие связи
        _context.LessonTypeTeachers.RemoveRange(existingCourceNames);
        _context.SaveChanges();

        // Создайте новые связи с новыми курсами
        foreach (var courceName in courcesId)
        {
            var courceNameTeacher = new PossibleCourseTeacher
            {
                TeacherId = item.Id,
                LessonTypeId = courceName
            };

            _context.LessonTypeTeachers.Add(courceNameTeacher);
        }

        await _context.SaveChangesAsync();




        return item;
    }
    */

    /*
	public async Task<Student> UpdateStudentWithCourcesNames(Student item, List<Guid> courcesId)
    {//VAR 2

        // Загрузите все существующие связи CourceNameTeacher для данного учителя
        var existingCourceNames = _context.LessonTypeStudents
            .Where(ct => ct.StudentId == item.Id)
            .ToList();

        // Удалите все существующие связи
        _context.LessonTypeStudents.RemoveRange(existingCourceNames);
        _context.SaveChanges();

        // Создайте новые связи с новыми курсами
        foreach (var courceName in courcesId)
        {
            var courceNameTeacher = new LessonTypeStudent
            {
                StudentId = item.Id,
                LessonTypeId = courceName
            };

            _context.LessonTypeStudents.Add(courceNameTeacher);
        }

        await _context.SaveChangesAsync();

        
        return item;

    }*/

    #region Delete

    public async Task<ApplicationUser> HardDeleteAsync(ApplicationUser item)
    {
        if (item == null) throw new Exception("User not found");
        if(!item.IsDeleted)
            await SoftDeleteAsync(item);

        _context.ApplicationUsers.Remove(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<ApplicationUser> SoftDeleteAsync(ApplicationUser item)
    {
        var roles = _context.UserRoles.Where(x => x.UserId == item.Id);
        _context.UserRoles.RemoveRange(roles);
        var teacher = item.UserTeacher;
        var student = item.UserStudent;

        if (teacher != null)
        {
            teacher.IsDeleted = true;
            teacher.GroupTeachers = new List<GroupTeacher>();


            teacher.PossibleCources = new List<PossibleCourseTeacher>();

            var scheduleTeacher = teacher.WorkingDays;

            //await DeleteStudentAsync(teacher.Id);
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

        }
        if (student != null)
        {
            student.IsDeleted = true;

            var groupIdsWithStudent = _context.Groups;


            foreach (var group in _context.Groups.Include(x => x.GroupStudents))
            {
                var groupStudents = group.GroupStudents;
                if (groupStudents != null && groupStudents.FirstOrDefault(x => x.Student == student) != null)
                {
                    _context.GroupStudents.Remove(groupStudents.FirstOrDefault(x => x.Student == student));
                    //group.GroupStudents.Remove(student);
                }
            }

            student.Groups = new List<GroupStudent>();
            student.PossibleCources = new List<PreferenceCourseStudent>();

            
            student.Lessons = new List<LessonStudent>();
            _context.Students.Update(student);

            await _context.SaveChangesAsync();

            foreach (var role in await _userManager.GetRolesAsync(item))
            {
                await _userManager.RemoveFromRoleAsync(item,role);
            }

            await _context.SaveChangesAsync();

        }

        //var lessons = _context.Lessons.Include(x => x.Creator).
        //    Where(x => x.Creator.Id == item.Id);
        //foreach (var lesson in lessons)
        //{
        //    lesson.Creator = null;
        //    _context.Lessons.Update(lesson);
        //}

        /*
        if(item.EnglishLevel != null)
        {
            item.EnglishLevel = null;
            try
            {
                //var a = await _context.EnglishLevels.FirstOrDefaultAsync(x => x.Users.Contains(item));
                //a.Users.Remove(item);
                //_context.EnglishLevels.Update(a);
            }
        catch (Exception ex) { }
            
        }*/

        item.IsDeleted = true;
        _context.ApplicationUsers.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<ApplicationUser> Restore(ApplicationUser item)
    {
        item.IsDeleted = false;
        _context.ApplicationUsers.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    #endregion



    public async Task<IQueryable<NotificationMessage>> GetCurrentUserNotifications()
    {
        var user = await GetCurrentUserAsync();
        List<NotificationMessage> userNotification = new List<NotificationMessage>();

        var notifications = _context.NotificationUsers.Include(x=>x.User)
            .Include(x=>x.NotificationMessage)
            .Where(x => x.User.Id == user.Id).Select(x=>x.NotificationMessage).OrderBy(x=>x.DateCreated);

        return notifications.AsQueryable();
    }


    //public Task<Lesson> ChangeLessonTime()
    //public Task<Lesson> ApproveChangeLessonTime()
    //public Task<Group> ApproveAddTeacherToGroup()
    //public Task<Group> ApproveAddStudentToGroup()

    //Add notification to Admin

}
