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
            
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x=>x.UserTeacher.PossibleCources).ThenInclude(x=>x.CourseName)
            .Include(x=>x.UserTeacher).ThenInclude(x=>x.Groups)//.ThenInclude(x=>x.GroupStudents)//.ThenInclude(x=>x.Student)
            .Include(x=>x.UserStudent)
            .Include(x=>x.UserStudent).ThenInclude(x=>x.Groups)//.ThenInclude(x=>x.Student)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups)//.ThenInclude(x => x.Group)//.ThenInclude(x=>x.GroupStudents).ThenInclude(x => x.Student)
            .Include(x=>x.UserStudent)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.CourseName)

            .FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }

    public async Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id)
    {
        var user = await _context.ApplicationUsers.Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.CourseName)
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher.PossibleCources).ThenInclude(x => x.CourseName)
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
            .Include(x => x.Groups).OrderBy(on => on.Id);

        return items;

    }
    public async Task<IQueryable<ApplicationUser>> GetAllAsync()
    {
        return _context.Users
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher.PossibleCources).ThenInclude(x => x.CourseName)
            .Include(x => x.UserTeacher).ThenInclude(x => x.Groups)
            .Include(x => x.UserStudent)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.CourseName)
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
        var userRegisterValidator = new TeacherRegisterValidator();

        var userRegister = _mapper.Map<TeacherDTO>(item);
        var validationResult = await userRegisterValidator.ValidateAsync(userRegister);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        item.ApplicationUser = dbUser;
        dbUser.UserTeacher = item;

        _context.Entry(item.ApplicationUser).State = EntityState.Unchanged;
        _context.Entry(dbUser.UserTeacher).State = EntityState.Unchanged;


        _context.ApplicationUsers.Update(dbUser);
        await _context.Teachers.AddAsync(item);

        var result = await _userManager.AddToRoleAsync(dbUser, "Teacher");
        await _context.SaveChangesAsync();
        if (!result.Succeeded) throw new Exception(result.Errors.ToString());
        var teacherInDb = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == user.Id);


        return teacherInDb == null ? throw new Exception("Error with save teacher in database") : teacherInDb;

    }
    public async Task<Student> CreateStudentAsync(ApplicationUser user, Student item)
    {
        //var dbUser = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ?? throw new Exception("User not found");
        var dbUser = user;

        var userRegisterValidator = new StudentRegisterValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        var student = _mapper.Map<Student>(item);
        student.ApplicationUser = dbUser;
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

        //if (item.EnglishLevelId != Guid.Empty) item.EnglishLevel = await _context.EnglishLevels.FirstOrDefaultAsync(x => x.Id == item.EnglishLevelId);
        
         if (_context.ApplicationUsers.FirstOrDefault(x => x.Email == user.Email) != null || user.Email == null) throw new Exception("User with such email alredy exists");
        if (_context.ApplicationUsers.FirstOrDefault(x => x.Login == user.Login) != null) throw new Exception("User with such login alredy exists");
        if (_context.ApplicationUsers.FirstOrDefault(x => x.Phone == user.Phone) != null) throw new Exception("User with such phone alredy exists");
        

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
    #endregion

    #region HZ - Update
    public async Task<Teacher> UpdateTeacherAsync(Teacher item)
    {
        //if (_context.Teachers.Any(x => x.Email == item.Email)) throw new Exception("Teacher with such email alredy exists");
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == item.Id) ?? throw new Exception("Teacher not found");
        var userRegisterValidator = new TeacherRegisterValidator();
        var teacherDTO = _mapper.Map<TeacherDTO>(item);
        var validationResult = await userRegisterValidator.ValidateAsync(teacherDTO);
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

    public async Task CreateAdminAsync(StudentDTO item = null)
    {
        var result = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Login == "AdminLogin");
        if (result != null) return;
        var admin = new ApplicationUser()
        {
            Login = "AdminLogin",
            UserName = "AdminLogin",
            FirstName = "AdminFirstName",
            LastName = "AdminLastName",
            Surname = "AdminSurname",
            Sex = "Male",
            Password = "AdminPassword123",
            OwnHashedPassword = HashProvider.ComputeHash("AdminPassword123"),
            IsDeleted = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        await _context.ApplicationUsers.AddAsync(admin);
        await _userManager.CreateAsync(admin, admin.Password.Trim());
        await _context.SaveChangesAsync();

        var re = await _userManager.AddToRoleAsync(admin, "Admin");
        if (re.Succeeded)


            await _context.SaveChangesAsync();
    }



    #endregion

    public async Task<Teacher> UpdateTeacherWithCourcesNames(Teacher item, List<Guid> courcesId)
    {
        
        //VAR 2

        // Загрузите все существующие связи CourceNameTeacher для данного учителя
        var existingCourceNames = _context.CourseNameTeachers
            .Where(ct => ct.TeacherId == item.Id)
            .ToList();

        // Удалите все существующие связи
        _context.CourseNameTeachers.RemoveRange(existingCourceNames);
        _context.SaveChanges();

        // Создайте новые связи с новыми курсами
        foreach (var courceName in courcesId)
        {
            var courceNameTeacher = new CourseNameTeacher
            {
                TeacherId = item.Id,
                CourseNameId = courceName
            };

            _context.CourseNameTeachers.Add(courceNameTeacher);
        }

        await _context.SaveChangesAsync();
        return item;
    }


	public async Task<Student> UpdateStudentWithCourcesNames(Student item, List<Guid> courcesId)
    {//VAR 2

        // Загрузите все существующие связи CourceNameTeacher для данного учителя
        var existingCourceNames = _context.CourseNameStudents
            .Where(ct => ct.StudentId == item.Id)
            .ToList();

        // Удалите все существующие связи
        _context.CourseNameStudents.RemoveRange(existingCourceNames);
        _context.SaveChanges();

        // Создайте новые связи с новыми курсами
        foreach (var courceName in courcesId)
        {
            var courceNameTeacher = new CourseNameStudent
            {
                StudentId = item.Id,
                CourseNameId = courceName
            };

            _context.CourseNameStudents.Add(courceNameTeacher);
        }

        await _context.SaveChangesAsync();
        return item;

    }

    #region Delete

    public async Task<ApplicationUser> HardDeleteAsync(ApplicationUser item)
    {
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
            var groups = _context.Groups.Include(x => x.Teacher).
                Where(x => x.Teacher.Id == teacher.Id);

            foreach (var group in groups)
            {
                group.Teacher = null;
                _context.Groups.Update(group);
            }

            foreach (var cource in teacher.PossibleCources)
            {
                //teacher.PossibleCources
            }


            var scheduleTeacher = teacher.WorkingDays;
            if (scheduleTeacher != null)
            {
                //_context.WorkingDays.RemoveRange(scheduleTeacher);
            }



        }
        if (student != null)
        {


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



            var schedulteStudent = student.WorkingDays;
            if (schedulteStudent != null)
            {
                //_context.WorkingDays.RemoveRange(schedulteStudent);
            }

        }

        var lessons = _context.Lessons.Include(x => x.Creator).
            Where(x => x.Creator.Id == item.Id);
        foreach (var lesson in lessons)
        {
            lesson.Creator = null;
            _context.Lessons.Update(lesson);
        }
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

        var notifications = _context.NotificationUsers.Include(x=>x.User).Include(x=>x.NotificationMessage).Where(x => x.User.Id == user.Id).Select(x=>x.NotificationMessage);

        return notifications.AsQueryable();
    }


    //public Task<Lesson> ChangeLessonTime()
    //public Task<Lesson> ApproveChangeLessonTime()
    //public Task<Group> ApproveAddTeacherToGroup()
    //public Task<Group> ApproveAddStudentToGroup()

    //Add notification to Admin

}
