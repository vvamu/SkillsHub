using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation.User;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Services.Repository;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using Spire.Xls.Core;
using System.Data;

namespace SkillsHub.Application.Services.Implementation;

public class UserService : AbstractTransactionService, IUserService
{
    protected readonly SignInManager<ApplicationUser> _signInManager;
    protected readonly UserManager<ApplicationUser> _userManager;
    protected readonly IAbstractLogModelService<BaseUserInfo> _baseUserInfoRepository;
    private readonly IUserRepository _userRepository;
    protected readonly IMapper _mapper;

    public UserService(ApplicationDbContext context,
         SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
         IMapper mapper, IAbstractLogModelService<BaseUserInfo> baseUserInfoRepository, IUserRepository userRepository) : base(context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = mapper;

        _baseUserInfoRepository = baseUserInfoRepository;
        _userRepository = userRepository;


        

    }

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
        //await CreateAdminAsync();
        if (!_context.Users.Any(x => x.UserName == item.UserName)) throw new Exception("User with such login not exists");
        ApplicationUser? user = await _context.Users.Where(x => x.UserName == item.UserName).FirstOrDefaultAsync()
            ?? throw new Exception("User not found");

        var has1 = user.OwnHashedPassword;
        if (!HashProvider.VerifyHash(item.Password, has1)) throw new Exception("Incorrect password");

        await _signInManager.SignInAsync(user, true);

        return user;
    }
    public async Task<ApplicationUser> SignInAsync(ApplicationUser item)
    {
        var loginUser = _mapper.Map<UserLoginDTO>(item);
        
        return await SignInAsync(loginUser);
    }
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }




    #endregion

    #region Get

    public async Task<IQueryable<NotificationMessage>> GetCurrentUserNotifications()
    {
        var user = await GetCurrentUserAsync();
        List<NotificationMessage> userNotification = new List<NotificationMessage>();

        var notifications = _context.NotificationUsers.Include(x => x.User)
            .Include(x => x.NotificationMessage)
            .Where(x => x.User.Id == user.Id).Select(x => x.NotificationMessage).OrderBy(x => x.DateCreated);

        return notifications.AsQueryable();
    }

    public async Task<ApplicationUser> GetById(Guid id)
    {
        
        ApplicationUser user;
        try
        {

            user = await _userRepository.GetAsync(id);
            //if (user?.UserTeacher != null)
            //{
            //    await _userManager.UpdateSecurityStampAsync(user);
            //    if (!user.UserTeacher.IsDeleted) await AddToRole(user, "Teacher");
            //    if (user.UserTeacher.IsDeleted) await _userManager.RemoveFromRoleAsync(user, "Teacher");
            //}
            //if (user?.UserStudent != null)
            //{
            //    var us = new ApplicationUser() { Id = user.Id };
            //    await _userManager.UpdateSecurityStampAsync(us);
            //    if (!user.UserStudent.IsDeleted) await AddToRole(user, "Student");
            //    if (user.UserStudent.IsDeleted) await _userManager.RemoveFromRoleAsync(user, "Student");
            //}
            var roles = await _userManager.GetRolesAsync(user);
            user.Roles = roles.ToArray();
            await _context.SaveChangesAsync();
            
        }
        catch (Exception ex) { return null; }


        return user;
    }
    public async Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id)
    {
        var user = await GetById(id);
        if (user == null) return null;
        var userCreateDTO = _mapper.Map<UserCreateDTO>(user);
        userCreateDTO.BaseUserInfoId = user?.UserInfo?.Id == null ? Guid.Empty: user.UserInfo.Id;
        var roles = await _userManager.GetRolesAsync(user);
        userCreateDTO.Roles = roles.Select(x => x.ToLower()).ToArray();

        //if (user?.UserTeacher != null) userCreateDTO.IsTeacher = true;
        //if (user?.UserStudent != null) userCreateDTO.IsStudent = true;

        return userCreateDTO;
    }
    public async Task<IQueryable<ApplicationUser>> GetItems()
    {
        var users = await _userRepository.GetItemsAsync();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            user.Roles = roles.ToArray();
        }
        return users;

    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userName = _signInManager.Context.User.Identity.Name;
        var users = await _context.Users.ToListAsync();
        if (userName == null)
        {
            await SignOutAsync();
            return null;
        }
        var dbUser = await _userManager.FindByNameAsync(userName);
        if (dbUser == null) dbUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (dbUser == null) return null;
        return await GetById(dbUser.Id);

    }

    #endregion


    public async Task<ApplicationUser> CreateAsync(UserCreateDTO item, bool baseUserInfoIsBase = false)
    {
        BaseUserInfo userInfoDb;
        var userInfo = _mapper.Map<BaseUserInfo>(item) ?? throw new Exception("Not correct model");
        ApplicationUser userDb = await _context.Users.FindAsync(userInfo.ApplicationUserId);
        if (userDb != null) throw new Exception("User already exists");


        var executionStrategy = CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await BeginTransactionAsync();
            try
            {
                userDb = await _userRepository.CreateAsync(item);
                userInfoDb = await _baseUserInfoRepository.CreateAsync(userInfo);

                var newApplicationBaseUserInfo = new ApplicationUserBaseUserInfo() { ApplicationUserId = userDb.Id, BaseUserInfoId = userInfoDb.Id, IsBase = baseUserInfoIsBase, Role = userInfoDb.Role };
                var dbRes = await _context.ApplicationUserBaseUserInfo.AddAsync(newApplicationBaseUserInfo);
                await _context.SaveChangesAsync();


                await UpdateRoles(userDb, item?.Roles?.ToList());

                var res = await GetById(userDb.Id);
                await CommitAsync(transaction);
                return res;

            }
            catch (Exception ex)
            {
                await RollbackAsync(transaction);
                throw new Exception("Не получилось обновить элемент. Попробуйте позже.", ex);
            }
        });
        return await GetById(userDb.Id);
    }

    public async Task<ApplicationUser> UpdateAsync(UserCreateDTO item)
    {
        if (item == null) throw new Exception("Model not set");
        //if (item.IsStudent == true && user.UserStudent == null) user.UserStudent = new Student() { ApplicationUser = user, IsDeleted = true };
        //if (item.IsTeacher == true && user.UserTeacher == null) user.UserTeacher = new Teacher() { ApplicationUser = user, IsDeleted = true };

        var executionStrategy = CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await BeginTransactionAsync();
            try
            {
                BaseUserInfo newUserInfo = _mapper.Map<BaseUserInfo>(item);
                newUserInfo.Id = item.BaseUserInfoId;

                try
                {
                    newUserInfo = await _baseUserInfoRepository.UpdateAsync(newUserInfo);
                }
                
                catch(Exception ex)
                {
                    if (!ex.Message.Equals("Entity with those properties already defined")) throw new Exception(ex.Message);
                }

                ApplicationUser userDb = _context.Users.AsNoTracking().FirstOrDefault( x=>x.Id ==item.Id);
                await UpdateRoles(userDb, item.Roles.ToList());
                try
                {
                    
                    userDb = await _userRepository.UpdateAsync(item);
                    _context.SaveChanges();
                    
                }
                catch(Exception ex)
                {

                }
                
                //BaseUserInfo? dbUserInfo = _context.BaseUserInfo.AsNoTracking().FirstOrDefault(x=>x.Id == item.BaseUserInfoId);
                
                
                //if (dbUserInfo == null || dbUserInfo.Equals(newUserInfo)) return;
                //var itemsByBaseUserInfo = GetApplicationUserBaseUserInfo(Guid.Empty, userDb.Id)?.Select(x => x.BaseUserInfo) ?? throw new Exception("No info in database");
                //if (itemsByBaseUserInfo.Count() == 0) throw new Exception("No info of connected account in database");

                
                var i = new ApplicationUserBaseUserInfo() { ApplicationUserId = item.Id, BaseUserInfoId = newUserInfo.Id ,IsBase = true};
                _context.ApplicationUserBaseUserInfo.Update(i);
                await _context.SaveChangesAsync();
                //var itemsToUpdate = _context.ApplicationUserBaseUserInfo.Where(x => x.BaseUserInfoId == dbUserInfo.Id); //поменялся baseUserInfo -> появился новый элемент -> все связи надо обновить на новые
                //foreach (var i in itemsToUpdate)
                //{
                //    i.BaseUserInfoId = updatedUserInfo.Id;
                //    _context.ApplicationUserBaseUserInfo.Update(i);
                //    await _context.SaveChangesAsync();
                //}

                
            }
            catch (Exception ex)
            {
                await RollbackAsync(transaction);
                throw new Exception("Не получилось обновить элемент. Попробуйте позже.", ex);
            }
            await _context.SaveChangesAsync();
            await CommitAsync(transaction);
        });

        return await GetById(item.Id);

    }

    

    public async Task<ApplicationUser> Restore(ApplicationUser item)
    {
        item.IsDeleted = false;
        _context.Users.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    #region Delete

    public async Task<ApplicationUser> DeleteAsync(ApplicationUser? item, bool isHardDelete = false)
    {
        if (item == null) throw new Exception("User not found");
        if (!isHardDelete) return await SoftDeleteAsync(item);
        return await HardDeleteAsync(item);

    }
    public async Task<ApplicationUser> DeleteAsync(Guid id, bool isHardDelete = false)
    {
        var item = await _context.Users.FindAsync(id);
        return await DeleteAsync(item, isHardDelete);

    }

    private async Task<ApplicationUser> HardDeleteAsync(ApplicationUser item)
    {

        
        var executionStrategy = CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await BeginTransactionAsync();
            try
            {
                var applicationUserBaseUsersInfo = _context.ApplicationUserBaseUserInfo.Where(x => x.ApplicationUserId == item.Id).ToList();
                var baseUsersInfo = applicationUserBaseUsersInfo.Select(x => x.BaseUserInfoId).Select(x => new BaseUserInfo() { Id = x }).ToList() ;

                _context.ApplicationUserBaseUserInfo.RemoveRange(applicationUserBaseUsersInfo);
                await _context.SaveChangesAsync();

                _context.BaseUserInfo.RemoveRange(baseUsersInfo);
                await _context.SaveChangesAsync();

                var student = await _context.Students.FirstOrDefaultAsync(x => x.ApplicationUserId == item.Id);
                if(student != null)
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.ApplicationUserId == item.Id);
                if (teacher != null)
                    _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();

                var emails = _context.EmailMessages.Where(x => x.SenderId == item.Id).ToList();
                _context.EmailMessages.RemoveRange(emails);
                await _context.SaveChangesAsync();

                var user = _context.Users.Find(item.Id);
                _context.Entry(user).State = EntityState.Deleted; // added row
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                      
            }
            catch (Exception ex)
            {
                await RollbackAsync(transaction);
                throw new Exception("Не получилось обновить элемент. Попробуйте позже.", ex);
            }
            await CommitAsync(transaction);
        });




        return item;
    }

    private async Task<ApplicationUser> SoftDeleteAsync(ApplicationUser item)
    {
        await UpdateRoles(item, null);
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
        _context.Users.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }



    #endregion

    private async Task UpdateRoles(ApplicationUser item, List<string> roles)
    {
        var roles2 = await _userManager.GetRolesAsync(item);
        await _userManager.RemoveFromRolesAsync(item, roles2);
        await _context.SaveChangesAsync();
        if (roles == null || roles.Count() == 0) return;

        foreach (var role in roles)
        {
            if (string.IsNullOrEmpty(role)) continue;
            switch (role.ToLower())
            {
                case "student":
                    {
                        
                        var st = new Student() { ApplicationUserId = item.Id, IsDeleted = false };
                        var dbStudent = await _context.Students.FirstOrDefaultAsync(x => x.ApplicationUserId == item.Id);
                        if (dbStudent== null) await _context.Students.AddAsync(st);

                        await _context.SaveChangesAsync();
                        await AddToRole(item, "Student");
                        break;
                    }
                case "teacher":
                    {
                        var tch = new Teacher() { ApplicationUserId = item.Id, IsDeleted = false };
                        var dbTeacher = await _context.Teachers.FirstOrDefaultAsync(x => x.ApplicationUserId == item.Id);
                        if (dbTeacher == null) await _context.Teachers.AddAsync(tch);

                        await _context.SaveChangesAsync();
                        await AddToRole(item, "Teacher");
                        break;
                    }
                default:
                    {
                        await AddToRole(item, role);
                        break;
                    }

            }
        }
    }

    private IQueryable<ApplicationUserBaseUserInfo>? GetApplicationUserBaseUserInfo(Guid? baseUserInfoId, Guid? userId)
    {
        var items = _context.ApplicationUserBaseUserInfo
            .Include(x => x.ApplicationUser)
            .Include(x => x.BaseUserInfo).AsQueryable();
        if (userId != Guid.Empty)
            items = items.Where(x => x.ApplicationUserId == userId);
        if (baseUserInfoId != Guid.Empty)
            items = items.Where(x => x.BaseUserInfoId == baseUserInfoId);

        return items;
    }

    private async Task AddToRole(ApplicationUser user, string role)
    {
        try
        {
            await _userManager.AddToRoleAsync(user, role);
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

    private async Task CreateAdminAsync()
    {
        var baseUsers = _context.BaseUserInfo.ToList();
        var users = _context.Users.Include(x=>x.ConnectedUsersInfo).ToList();
        var baseUsersToDel = users.SelectMany(x => x.ConnectedUsersInfo).Select(x => x.BaseUserInfoId).ToList();
        var ids = baseUsers.Select(x => x.Id);
        var except = ids.Except(baseUsersToDel).Select(x => new BaseUserInfo() { Id = x }).ToList();
        _context.BaseUserInfo.RemoveRange(except);
        await _context.SaveChangesAsync();
        baseUsers = _context.BaseUserInfo.ToList();

        //var baseUsersToDel = baseUsers.Where(x => x.FirstName.ToLower().Contains("admin"));
        //_context.BaseUserInfo.RemoveRange(baseUsersToDel);
        //baseUsers = _context.BaseUserInfo.ToList();



        await _context.SaveChangesAsync();
        var userDb = users.FirstOrDefault(x => x.UserName == "skills-admin");
        if (userDb != null)
        {
            await DeleteAsync(userDb, true);
        }

        var user = new UserCreateDTO()
        {
            UserName = "skills-admin",
            Password = "AdminPassword123",
            IsDeleted = false,
            FirstName = "Ахмед",
            MiddleName = "Бал",
            Surname = "AdminSurname",
            Sex = "Male",
            BirthDate = new DateTime(1994, 08, 14),
            PhonesArray = new string[] { "+375 44 707-40-07" }

        };

        try
        {
            user.Roles = new[] { "Admin" };
            userDb = await CreateAsync(user,true);
        }
        catch (Exception ex)
        {

        }
    }



    #region ImageUpload



    #endregion
    //public Task<Lesson> ChangeLessonTime()
    //public Task<Lesson> ApproveChangeLessonTime()
    //public Task<Group> ApproveAddTeacherToGroup()
    //public Task<Group> ApproveAddStudentToGroup()

    //Add notification to Admin

}
