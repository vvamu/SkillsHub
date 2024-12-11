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
using Spire.Xls.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace SkillsHub.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<ApplicationUser> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBaseUserInfoService _baseUserInfoService;

    public UserService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        //RoleManager<ApplicationUser> roleManager,
        ApplicationDbContext context, IMapper mapper, IBaseUserInfoService baseUserInfoService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        //_roleManager = roleManager;
        _context = context;
        _mapper = mapper;
        _baseUserInfoService = baseUserInfoService;
    }
    #region Get

    public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
    {        
        var user = await _context?.ApplicationUsers?.AsNoTracking()
            .Include(x=>x.UserTeacher).ThenInclude(x=>x.PossibleCources).ThenInclude(x=>x.LessonType)
            .Include(x => x.UserTeacher).ThenInclude(x => x.Groups)
            .Include(x => x.UserTeacher).ThenInclude(x => x.PossibleCources)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups).ThenInclude(x => x.Group).ThenInclude(x=>x.Lessons)//.ThenInclude(x=>x.GroupStudents).ThenInclude(x => x.Student)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent).ThenInclude(x => x.PossibleCources).ThenInclude(x => x.LessonType)
            .Include(x=>x.ConnectedUsersInfo).ThenInclude(x=>x.BaseUserInfo)
            .Include(x=>x.UserWorkingDays)
            .FirstOrDefaultAsync(x => x.Id == id);
        _context.ChangeTracker.Clear();
        if (user == null) return null;
        
        try
        {
            _context.ApplicationUsers.Attach(user);
            _context.Entry(user).State = EntityState.Detached;
        }
        catch (Exception ex) { }
        
        try
        {

           

            if(user.UserTeacher != null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                if (!user.UserTeacher.IsDeleted) await _userManager.AddToRoleAsync(user, "Teacher");
                if (user.UserTeacher.IsDeleted) await _userManager.RemoveFromRoleAsync(user, "Teacher");
            }
            if(user.UserStudent != null)
            {
                var us = new ApplicationUser() { Id = user.Id };
                await _userManager.UpdateSecurityStampAsync(us);
                if (!user.UserStudent.IsDeleted) await _userManager.AddToRoleAsync(user, "Student");
                if (user.UserStudent.IsDeleted) await _userManager.RemoveFromRoleAsync(user, "Student");
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }


        return user;
    }
    public async Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        if (user == null) return null;
        var userCreateDTO = _mapper.Map<UserCreateDTO>(user);
        userCreateDTO.BaseUserInfoId = user.UserInfo.Id;
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
            .OrderBy(on => on.Id);

        return items;

    }
    public async Task<IQueryable<ApplicationUser>> GetAllAsync()
    {
        var users =  _context.Users
            .Include(x => x.UserTeacher)//.ThenInclude(x => x.Lessons)
            .Include(x => x.UserTeacher.PossibleCources).ThenInclude(x => x.LessonType)
            .Include(x => x.UserTeacher)
            .Include(x => x.UserStudent)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent.PossibleCources).ThenInclude(x => x.LessonType)
            .Include(x => x.ConnectedUsersInfo).ThenInclude(x=>x.BaseUserInfo)
            .OrderBy(x => x.Id);
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
           user.Roles = String.Join(";", roles);
        }
        return users;

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


        _context.Entry(item.ApplicationUser).State = EntityState.Unchanged;
        _context.Entry(dbUser.UserTeacher).State = EntityState.Unchanged;
        //if (!result.Succeeded) throw new Exception(result.Errors.ToString());
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
        var userInfo = _mapper.Map<BaseUserInfo>(item);
        user.UserName = user.Login;


        #region Validators
        var userRegisterValidator = new UserCreateDTOValidator();
        var userValidationResult = await userRegisterValidator.ValidateAsync(item);
        if (!userValidationResult.IsValid)
        {
            var errors = userValidationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        if (_context.ApplicationUsers.FirstOrDefault(x => x.Login == user.Login) != null) throw new Exception("User with this login already exists");
        #endregion

        string hashedPassword = HashProvider.ComputeHash(item.Password.Trim());
        user.OwnHashedPassword = hashedPassword;

        var result = await _context.ApplicationUsers.AddAsync(user);
        await _userManager.UpdateSecurityStampAsync(user);

        await _context.SaveChangesAsync();


        if (item.IsStudent == true)
        {
            var st = new Student() { ApplicationUserId = result.Entity.Id, IsDeleted = false }; await _context.Students.AddAsync(st); await _context.SaveChangesAsync();

            try
            {
                await _userManager.AddToRoleAsync(result.Entity, "Student");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is ApplicationUser)
                    {
                        var proposedValues = entry.CurrentValues; // Предложенные значения
                        var databaseValues = entry.GetDatabaseValues(); // Значения из базы данных

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // Решение о том, какое значение использовать
                            // Можно выбрать proposedValue, databaseValue или применить свою логику объединения значений

                            // Например, просто обновляем оригинальные значения до значений из базы данных
                            entry.OriginalValues.SetValues(proposedValues);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Concurrency conflict occurred for entity type: " + entry.Entity.GetType().Name);
                    }
                }

                // Повторно пытаемся сохранить изменения
                _context.SaveChanges();
            }
        }
        if (item.IsTeacher == true)
        {
            var tch = new Teacher() { ApplicationUserId = result.Entity.Id, IsDeleted = false }; await _context.Teachers.AddAsync(tch); await _context.SaveChangesAsync();


            try
            {

                await _userManager.AddToRoleAsync(result.Entity, "Teacher");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is ApplicationUser)
                    {
                        var proposedValues = entry.CurrentValues; // Предложенные значения
                        var databaseValues = entry.GetDatabaseValues(); // Значения из базы данных

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // Решение о том, какое значение использовать
                            // Можно выбрать proposedValue, databaseValue или применить свою логику объединения значений

                            // Например, просто обновляем оригинальные значения до значений из базы данных
                            entry.OriginalValues.SetValues(proposedValues);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Concurrency conflict occurred for entity type: " + entry.Entity.GetType().Name);
                    }
                }

                // Повторно пытаемся сохранить изменения
                _context.SaveChanges();
            }
        }
        return await GetUserByIdAsync(result.Entity.Id);
    }

    public async Task<ApplicationUser> UpdateUserAsync(UserCreateDTO item)
    {
        #region CreateUser
        var dbUser = await GetUserByIdAsync(item.Id);
      
        if (dbUser == null ) throw new Exception("User was not found in database");

        var connectedUserInfo = dbUser;
        var user = _mapper.Map<ApplicationUser>(item);
        user.UserName = user.Login;
        user.OwnHashedPassword = HashProvider.ComputeHash(item.Password.Trim());


        #region Validators
        var userRegisterValidator = new UserCreateDTOValidator();
        var userValidationResult = await userRegisterValidator.ValidateAsync(item);
        if (!userValidationResult.IsValid)
        {
            var errors = userValidationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        if (_context.ApplicationUsers.Where(x => x.Login == user.Login).Count() > 1) throw new Exception("User with this login already exists");
        #endregion

        if (!HashProvider.VerifyHash(item.Password.Trim(), dbUser.OwnHashedPassword)) throw new Exception("Password not equal");
        if (!string.IsNullOrEmpty(item.PasswordChanged) && !string.IsNullOrEmpty(item.PasswordChangedConfirm))
        {
            if (item.PasswordChanged != item.PasswordChangedConfirm) throw new Exception("Changed password values ​​are not equal");
            user.OwnHashedPassword = HashProvider.ComputeHash(item.PasswordChangedConfirm.Trim());
        }

        dbUser = user;

        var result =  _context.ApplicationUsers.Update(dbUser);

        var saved = false;
        while (!saved)
        {
            try
            {
                // Attempt to save changes to the database
                _context.SaveChanges();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is ApplicationUser)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            if (proposedValue != null && !proposedValue.Equals(databaseValue))
                            {
                                proposedValues[property] = proposedValue; // Keep the proposed value
                            }

                            // TODO: decide which value should be written to database
                            // proposedValues[property] = <value to be saved>;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
        }

        #endregion

       



        if (item.IsStudent == true && user.UserStudent == null) user.UserStudent = new Student() { ApplicationUser = user, IsDeleted = true };
        if (item.IsTeacher == true && user.UserTeacher == null) user.UserTeacher = new Teacher() { ApplicationUser = user, IsDeleted = true };


        return await GetUserByIdAsync(user.Id);

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
        var resultAdmin = _context.ApplicationUsers.FirstOrDefault(x => x.Login == "AdminLogin");
        var resultAdminInfo = _context.BaseUserInfo.FirstOrDefault(x => x.FirstName == "AdminFirstName");
        if (resultAdmin != null && resultAdminInfo != null) return;

        if (resultAdminInfo == null)
        {
            resultAdminInfo = _context.BaseUserInfo.FirstOrDefault(x => x.FirstName == "AdminFirstName") ?? new BaseUserInfo()
            {

                FirstName = "AdminFirstName",
                LastName = "AdminLastName",
                Surname = "AdminSurname",
                Sex = "Male",
                //IsBase = true,
            };
            await _context.BaseUserInfo.AddAsync(resultAdminInfo);
            await _context.SaveChangesAsync();
        }
        if (resultAdmin == null)
        {
            resultAdmin = new ApplicationUser()
            {
                Login = "AdminLogin",
                UserName = "AdminLogin",
                //Password = "AdminPassword123",
                OwnHashedPassword = HashProvider.ComputeHash("AdminPassword123"),
                IsDeleted = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var res = await _context.ApplicationUsers.AddAsync(resultAdmin);
            await _context.SaveChangesAsync();
            resultAdmin = res.Entity;
            try
            {
                var re = await _userManager.AddToRoleAsync(resultAdmin, "Admin");
                await _context.SaveChangesAsync();
            } catch { }

        }
        var applicationUserBaseUserInfo = new ApplicationUserBaseUserInfo() { ApplicationUserId = resultAdmin.Id, BaseUserInfoId = resultAdminInfo.Id };
        await _context.ApplicationUserBaseUserInfo.AddAsync(applicationUserBaseUserInfo);

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
        await _context.SaveChangesAsync();
        var teacher = item.UserTeacher;
        var student = item.UserStudent;

        if (teacher != null)
        {
            teacher.IsDeleted = true;
            //teacher.Groups
            //teacher.PossibleCources = new List<LessonTypeTeacher>();

            //var scheduleTeacher = teacher.WorkingDays;

            //await DeleteStudentAsync(teacher.Id);
            
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

        }
        if (student != null)
        {
            student.IsDeleted = true;

            /*
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
            student.PossibleCources = new List<LessonTypeStudent>();

            
            student.Lessons = new List<LessonStudent>();
            */
            _context.Students.Update(student);
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
