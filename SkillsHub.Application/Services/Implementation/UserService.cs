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
        return await _context.Users.Include(x=>x.UserStudent).ThenInclude(x=>x.Lessons).Include(x=>x.UserTeacher).ThenInclude(x => x.Lessons).
            FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User not found");
    }

    #endregion
    #region Create 
    public async Task<Teacher> CreateTeacherAsync(Guid userId, TeacherDTO item)
    {
        var dbUser = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ?? throw new Exception("User not found");

        var userRegisterValidator = new TeacherRegisterValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }


        var teacher = _mapper.Map<Teacher>(item);
        var str = "";
        teacher.ApplicationUser = dbUser;

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        var result = await _userManager.AddToRoleAsync(dbUser, "Teacher");
        await _context.SaveChangesAsync();
        if (!result.Succeeded) throw new Exception(result.Errors.ToString());
        var teacherInDb = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == userId);


        return teacherInDb == null ? throw new Exception("Error with save teacher in database") : teacherInDb;

    }
    public async Task<Student> CreateStudentAsync(Guid userId, StudentDTO item)
    {
        var dbUser = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync() ?? throw new Exception("User not found");

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

        if (_context.Users.Any(x => x.Email == user.Email) && user.Email != null) throw new Exception("User with such email alredy exists");
        if (_context.Users.Any(x => x.Login == user.Login)) throw new Exception("User with such login alredy exists");
        if (_context.Users.Any(x => x.Phone == user.Phone)) throw new Exception("User with such phone alredy exists");

        string hashedPassword = HashProvider.ComputeHash(user.Password.Trim());
        user.OwnHashedPassword = hashedPassword;
        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded) throw new Exception(string.Concat(result.Errors.Select(x=>x.Description)));
        if (item.IsStudent == true) user.UserStudent = new Student() { ApplicationUser = user };
        if (item.IsTeacher == true) user.UserTeacher = new Teacher() { ApplicationUser = user };
        await _context.SaveChangesAsync();

        _context.Update(user);
        await _context.SaveChangesAsync();

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

        _context.Update(item);
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
        var dbItem = await _context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User not found");
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
        ApplicationUser? user = await _context.Users.Where(x => x.Login == item.Login).FirstOrDefaultAsync()
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
            throw new Exception(errorsString);
        }

        ApplicationUser? user = await _context.Users.Where(x => x.Login == item.Login).FirstOrDefaultAsync()
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
        var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == "AdminLogin");
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
            IsAdmin = true,
            IsDeleted = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        await _context.Users.AddAsync(admin);
        await _userManager.CreateAsync(admin, admin.Password.Trim());
        await _context.SaveChangesAsync();

        var re = await _userManager.AddToRoleAsync(admin, "Admin");
        if (re.Succeeded)


            await _context.SaveChangesAsync();
    }



    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userName = _signInManager.Context.User.Identity.Name;
        if (userName == null) throw new Exception();
        var dbUser = await _userManager.FindByNameAsync(userName);
        return dbUser;

    }
    public async Task<bool> IsAdminAsync()
    {
        var user = await GetCurrentUserAsync();
        return await _userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<IQueryable<ApplicationUser>> GetAllAsync()
    {
        return _context.Users.OrderBy(x => x.Id);
    }

    #endregion


    public async Task<IQueryable<StudentDTO>> GetAllStudentsAsync()
    {
        var items = _context.Students.Include(x => x.Lessons).OrderBy(on => on.Id);
        var users = _context.Users.Include(x => x.UserStudent).Where(x => x.UserStudent != null).OrderBy(on => on.Id);
        var mappingItems = _mapper.Map<List<StudentDTO>>(items).AsQueryable();
        mappingItems = _mapper.Map<List<StudentDTO>>(users).AsQueryable();

        //mappingItems = _mapper.Map< IQueryable < StudentDTO >> (users).AsQueryable();


        
        return mappingItems;
    }
    public async Task<IQueryable<Teacher>> GetTeachersByLessonTypeAsync(Guid lessonTypeId)
    {
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x=>x.Id == lessonTypeId);
        if (lessonType == null) throw new Exception("Lesson type not found");
        return _context.Teachers.Include(x=>x.PossibleCources).Where(x => x.PossibleCources.Any(x => x.Id == lessonTypeId));
    }

    public async Task<Teacher> CreatePossibleCourcesNamesToTeacherAsync(Guid itemId, List<Guid> courcesId)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == itemId) ?? throw new Exception("Teacher not found. Maybe teacher is created but cources were not add");
        var courcesNamesList = new List<CourceName>();
        foreach (var courceNameId in courcesId)
        {
            var dbItem = await _context.CourceNames.FirstOrDefaultAsync(x => x.Id == courceNameId) ?? throw new Exception("Cource`s name not found");
            courcesNamesList.Add(dbItem);
        }
        teacher.PossibleCources = courcesNamesList;

        _context.Update(teacher);
        await _context.SaveChangesAsync();
        return teacher;

    }
    public async Task<Student> CreatePossibleCourcesNamesToStudentAsync(Guid itemId, List<Guid> courcesId)
    {
        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == itemId) ?? throw new Exception("Teacher not found. Maybe teacher is created but cources were not add");
        var courcesNamesList = new List<CourceName>();
        foreach (var courceNameId in courcesId)
        {
            var dbItem = await _context.CourceNames.FirstOrDefaultAsync(x => x.Id == courceNameId) ?? throw new Exception("Cource`s name not found");
            courcesNamesList.Add(dbItem);
        }
        student.PossibleCources = courcesNamesList;

        _context.Update(student);
        await _context.SaveChangesAsync();
        return student;

    }

    public IQueryable<TeacherDTO> GetAllTeachers()
    {
        var items = _context.Teachers.Include(x => x.Lessons).OrderBy(on => on.Id);
        var users = _context.Users.Include(x => x.UserTeacher).Where(x => x.UserTeacher != null).OrderBy(on => on.Id);

        var mappingItems = _mapper.Map<List<TeacherDTO>>(items).AsQueryable();
        mappingItems = _mapper.Map<List<TeacherDTO>>(users).AsQueryable();
        
        return mappingItems;

    }
}
